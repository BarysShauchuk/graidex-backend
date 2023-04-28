using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.Infrastructure.ResultObjects.Generic;
using Graidex.Application.Infrastructure.ResultObjects.NonGeneric;
using Graidex.Application.Infrastructure.ValidationFailure;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Authentication
{
    /// <summary>
    /// Authentication Service.
    /// </summary>
    public class AuthenticationService : IStudentAuthenticationService, ITeacherAuthenticationService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IValidator<StudentDto> studentDtoValidator;
        private readonly IValidator<TeacherDto> teacherValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="studentRepository">Repository for <see cref="Student"/>.</param>
        /// <param name="teacherRepository">Repository for <see cref="Teacher"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/> instance for getting configuration values.</param>
        /// <param name="mapper"><see cref="IMapper"/> instance for mapping DTOs and domain models.</param>
        /// <param name="studentDtoValidator"><see cref="IValidator{T}"/> instance for <see cref="StudentDto"/> validation.</param>
        /// <param name="teacherValidator"><see cref="IValidator{T}"/> instance for <see cref="TeacherDto"/> validation.</param>
        public AuthenticationService(
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            IConfiguration configuration,
            IMapper mapper,
            IValidator<StudentDto> studentDtoValidator,
            IValidator<TeacherDto> teacherValidator)
        {
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.configuration = configuration;
            this.mapper = mapper;
            this.studentDtoValidator = studentDtoValidator;
            this.teacherValidator = teacherValidator;
        }

        /// <inheritdoc/>
        public async Task<Result> RegisterStudent(StudentDto student)
        {
            var result = new ResultFactory();

            var validationResult = await this.studentDtoValidator.ValidateAsync(student);
            if (!validationResult.IsValid)
            {
                return result.ValidationFailure(validationResult.Errors);
            }

            bool studentExists = this.studentRepository
                .GetAll()
                .Any(x => x.Email == student.AuthInfo.Email);

            if (studentExists)
            {
                return result.Failure(
                    $"Student with email \"{student.AuthInfo.Email}\" already exists.");
            }

            var dbStudent = this.mapper.Map<Student>(student);
            dbStudent.PasswordHash = BCrypt.Net.BCrypt.HashPassword(student.AuthInfo.Password);
            await this.studentRepository.Add(dbStudent);

            return result.Success();
        }

        /// <inheritdoc/>
        public Task<Result<string>> LoginStudent(UserAuthDto student)
        {
            var result = new ResultFactory<string>();

            var keyToken = this.configuration["AppSettings:Token"];
            if (keyToken is null)
            {
                throw new InvalidOperationException("Key token configuration string isn't found.");
            }

            var dbStudent = this.studentRepository
                .GetAll()
                .Select(x => new { x.Email, x.PasswordHash })
                .SingleOrDefault(x => x.Email == student.Email);

            if (dbStudent is null)
            {
                return Task.FromResult(result.Failure("Student not found."));
            }

            if (!BCrypt.Net.BCrypt.Verify(student.Password, dbStudent.PasswordHash))
            {
                return Task.FromResult(result.Failure("Wrong password."));
            }

            var token = CreateStudentToken(student, keyToken);
            return Task.FromResult(result.Success(token));
        }

        /// <inheritdoc/>
        public async Task<Result> RegisterTeacher(TeacherDto teacher)
        {
            var result = new ResultFactory();

            var validationResult = await this.teacherValidator.ValidateAsync(teacher);
            if (!validationResult.IsValid)
            {
                return result.ValidationFailure(validationResult.Errors);
            }

            bool teacherExists = this.teacherRepository
                .GetAll()
                .Any(x => x.Email == teacher.AuthInfo.Email);

            if (teacherExists)
            {
                return result.Failure(
                    $"Teacher with email \"{teacher.AuthInfo.Email}\" already exists.");
            }

            var dbTeacher = this.mapper.Map<Teacher>(teacher);
            dbTeacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(teacher.AuthInfo.Password);
            await this.teacherRepository.Add(dbTeacher);

            return result.Success();
        }

        /// <inheritdoc/>
        public Task<Result<string>> LoginTeacher(UserAuthDto teacher)
        {
            var result = new ResultFactory<string>();

            var keyToken = this.configuration["AppSettings:Token"];
            if (keyToken is null)
            {
                throw new InvalidOperationException("Key token configuration string isn't found.");
            }

            var dbTeacher = this.teacherRepository
                .GetAll()
                .Select(x => new { x.Email, x.PasswordHash })
                .SingleOrDefault(x => x.Email == teacher.Email);

            if (dbTeacher is null)
            {
                return Task.FromResult(result.Failure("Teacher not found."));
            }

            if (!BCrypt.Net.BCrypt.Verify(teacher.Password, dbTeacher.PasswordHash))
            {
                return Task.FromResult(result.Failure("Wrong password."));
            }

            var token = CreateTeacherToken(teacher, keyToken);
            return Task.FromResult(result.Success(token));
        }

        private static string CreateStudentToken(UserAuthDto student, string keyToken)
        {
            return CreateToken(student, keyToken, 12, new[] { "Student" });
        }

        private static string CreateTeacherToken(UserAuthDto teacher, string keyToken)
        {
            return CreateToken(teacher, keyToken, 24, new[] { "Teacher" });
        }

        private static string CreateToken(UserAuthDto user, string keyToken, int hoursToExpiration, IEnumerable<string>? roles = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (roles is not null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyToken));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(hoursToExpiration),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

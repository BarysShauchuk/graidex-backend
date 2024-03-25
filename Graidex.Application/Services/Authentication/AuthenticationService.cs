using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OneOf.Types;
using OneOf;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.Interfaces;
using MediatR;
using Graidex.Application.Notifications.Authentication.Login;
using Microsoft.Extensions.Logging;

namespace Graidex.Application.Services.Authentication
{
    // TODO: Refactor
    /// <summary>
    /// Authentication Service.
    /// </summary>
    public class AuthenticationService : IStudentAuthenticationService, ITeacherAuthenticationService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IValidator<CreateStudentDto> studentDtoValidator;
        private readonly IValidator<CreateTeacherDto> teacherValidator;
        private readonly IMediator mediator;
        private readonly ILogger<AuthenticationService> logger;
        private readonly ICurrentUserService currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="studentRepository">Repository for <see cref="Student"/>.</param>
        /// <param name="teacherRepository">Repository for <see cref="Teacher"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/> instance for getting configuration values.</param>
        /// <param name="mapper"><see cref="IMapper"/> instance for mapping DTOs and domain models.</param>
        /// <param name="studentDtoValidator"><see cref="IValidator{T}"/> instance for <see cref="CreateStudentDto"/> validation.</param>
        /// <param name="teacherValidator"><see cref="IValidator{T}"/> instance for <see cref="CreateTeacherDto"/> validation.</param>
        public AuthenticationService(
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            IConfiguration configuration,
            IMapper mapper,
            IValidator<CreateStudentDto> studentDtoValidator,
            IValidator<CreateTeacherDto> teacherValidator,
            IMediator mediator,
            ILogger<AuthenticationService> logger,
            ICurrentUserService currentUserService)
        {
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.configuration = configuration;
            this.mapper = mapper;
            this.studentDtoValidator = studentDtoValidator;
            this.teacherValidator = teacherValidator;
            this.mediator = mediator;
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        /// <inheritdoc/>
        public async Task<OneOf<Success, ValidationFailed, UserAlreadyExists>> RegisterStudentAsync(CreateStudentDto student)
        {
            var validationResult = await this.studentDtoValidator.ValidateAsync(student);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            bool studentExists = this.studentRepository
                .GetAll()
                .Any(x => x.Email == student.Email);

            if (studentExists)
            {
                return new UserAlreadyExists(
                    $"Student with email \"{student.Email}\" already exists.");
            }

            var dbStudent = this.mapper.Map<Student>(student);
            dbStudent.PasswordHash = BCrypt.Net.BCrypt.HashPassword(student.Password);
            await this.studentRepository.Add(dbStudent);

            return new Success();
        }

        /// <inheritdoc/>
        public async Task<OneOf<string, UserNotFound, WrongPassword>> LoginStudentAsync(UserAuthDto student)
        {
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
                return new UserNotFound($"Student with email \"{student.Email}\" is not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(student.Password, dbStudent.PasswordHash))
            {
                return new WrongPassword();
            }

            var token = CreateStudentToken(student, keyToken);

            await this.NotifyUserNewLogin(student.Email, nameof(Student));

            return token;
        }

        /// <inheritdoc/>
        public async Task<OneOf<Success, ValidationFailed, UserAlreadyExists>> RegisterTeacherAsync(CreateTeacherDto teacher)
        {
            var validationResult = await this.teacherValidator.ValidateAsync(teacher);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            bool teacherExists = this.teacherRepository
                .GetAll()
                .Any(x => x.Email == teacher.Email);

            if (teacherExists)
            {
                return new UserAlreadyExists(
                    $"Teacher with email \"{teacher.Email}\" already exists.");
            }

            var dbTeacher = this.mapper.Map<Teacher>(teacher);
            dbTeacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(teacher.Password);
            await this.teacherRepository.Add(dbTeacher);

            return new Success();
        }

        /// <inheritdoc/>
        public async Task<OneOf<string, UserNotFound, WrongPassword>> LoginTeacherAsync(UserAuthDto teacher)
        {
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
                return new UserNotFound($"Teacher with email \"{teacher.Email}\" is not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(teacher.Password, dbTeacher.PasswordHash))
            {
                return new WrongPassword();
            }

            var token = CreateTeacherToken(teacher, keyToken);

            await this.NotifyUserNewLogin(teacher.Email, nameof(Teacher));

            return token;
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
                new Claim(ClaimTypes.Name, user.Email),
            };

            if (roles is not null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                if (roles.Count() == 1)
                {
                    claims.Add(
                        new Claim(ClaimTypes.NameIdentifier, 
                        UserIdentity.Get(user.Email, roles.First())));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyToken));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(hoursToExpiration),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private async Task NotifyUserNewLogin(string email, string role)
        {
            try
            {
                await this.mediator.Publish(new NewLoginNotification
                {
                    Email = email,
                    Role = role,
                    Data = new()
                    {
                        IpAddress = this.currentUserService.GetIpAddress(),
                        LoginTime = DateTimeOffset.UtcNow
                    }
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while publishing login notification.");
            }
        }
    }
}

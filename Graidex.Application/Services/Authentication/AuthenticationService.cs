using Graidex.Application.DTOs.Authentication;
using Graidex.Application.ResultObjects;
using Graidex.Application.ResultObjects.Generic;
using Graidex.Application.ResultObjects.NonGeneric;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<Teacher> teacherRepository;

        public AuthenticationService(
            IRepository<Student> studentRepository,
            IRepository<Teacher> teacherRepository)
        {
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
        }

        public async Task<Result> RegisterStudent(StudentAuthDto student)
        {
            var result = new ResultFactory();

            bool studentExists = this.studentRepository
                .GetAll()
                .Any(x => x.Email == student.Email);

            if (studentExists)
            {
                return result.Failure($"Student with email \"{student.Email}\" already exists.");
            }

            Student dbStudent = new Student
            {
                Email = student.Email,
                Password = student.Password,
                Name = student.Name,
                Surname = student.Surname,
                CustomId = student.CustomId
            };

            await this.studentRepository.Add(dbStudent);

            return result.Success();
        }

        public async Task<Result<string>> LoginStudent(UserAuthDto student, string keyToken)
        {
            var result = new ResultFactory<string>();

            var dbStudent = this.studentRepository
                .GetAll()
                .Select(x => new { x.Email, x.Password })
                .SingleOrDefault(x => x.Email == student.Email);

            if (dbStudent is null)
            {
                return result.Failure("Student not found.");
            }

            if (dbStudent.Password != student.Password)
            {
                return result.Failure("Wrong password.");
            }

            var token = CreateStudentToken(student, keyToken);
            return result.Success(token);
        }

        private static string CreateStudentToken(UserAuthDto student, string keyToken)
        {
            return CreateToken(student, keyToken, new[] { "Student" });
        }

        private static string CreateToken(UserAuthDto user, string keyToken, IEnumerable<string>? roles = null)
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
                expires: DateTime.Now.AddHours(4),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

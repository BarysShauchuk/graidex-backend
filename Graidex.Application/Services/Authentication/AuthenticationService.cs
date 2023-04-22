﻿using Graidex.Application.DTOs.Authentication;
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
    /// <summary>
    /// Authentication Service.
    /// </summary>
    public class AuthenticationService : IStudentAuthenticationService, ITeacherAuthenticationService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="studentRepository">Repository for <see cref="Student"/>.</param>
        /// <param name="teacherRepository">Repository for <see cref="Teacher"/>.</param>
        public AuthenticationService(
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository)
        {
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Task<Result<string>> LoginStudent(UserAuthDto student, string keyToken)
        {
            var result = new ResultFactory<string>();

            var dbStudent = this.studentRepository
                .GetAll()
                .Select(x => new { x.Email, x.Password })
                .SingleOrDefault(x => x.Email == student.Email);

            if (dbStudent is null)
            {
                return Task.FromResult(result.Failure("Student not found."));
            }

            if (dbStudent.Password != student.Password)
            {
                return Task.FromResult(result.Failure("Wrong password."));
            }

            var token = CreateStudentToken(student, keyToken);
            return Task.FromResult(result.Success(token));
        }

        /// <inheritdoc/>
        public async Task<Result> RegisterTeacher(TeacherAuthDto teacher)
        {
            var result = new ResultFactory();

            bool teacherExists = this.teacherRepository
                .GetAll()
                .Any(x => x.Email == teacher.Email);

            if (teacherExists)
            {
                return result.Failure($"Teacher with email \"{teacher.Email}\" already exists.");
            }

            Teacher dbTeacher = new Teacher
            {
                Email = teacher.Email,
                Password = teacher.Password,
                Name = teacher.Name,
                Surname = teacher.Surname
            };

            await this.teacherRepository.Add(dbTeacher);

            return result.Success();
        }

        /// <inheritdoc/>
        public Task<Result<string>> LoginTeacher(UserAuthDto teacher, string keyToken)
        {
            var result = new ResultFactory<string>();

            var dbTeacher = this.teacherRepository
                .GetAll()
                .Select(x => new { x.Email, x.Password })
                .SingleOrDefault(x => x.Email == teacher.Email);

            if (dbTeacher is null)
            {
                return Task.FromResult(result.Failure("Teacher not found."));
            }

            if (dbTeacher.Password != teacher.Password)
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

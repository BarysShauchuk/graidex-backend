using AutoMapper;
using FluentValidation;
using Graidex.Application.AutoMapperProfiles;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Tests.Fakes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Services
{
    internal class AuthenticationServiceTests
    {
        private FakeStudentRepository studentRepository;
        private FakeTeacherRepository teacherRepository;
        private AuthenticationService authenticationService;

        [SetUp]
        public void Setup()
        {
            this.studentRepository = new FakeStudentRepository();
            this.teacherRepository = new FakeTeacherRepository();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock
                .Setup(x => x["AppSettings:Token"])
                .Returns("Secret key token");

            var studentDtoValidatorMock = new Mock<IValidator<CreateStudentDto>>();
            studentDtoValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<CreateStudentDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var teacherDtoValidatorMock = new Mock<IValidator<CreateTeacherDto>>();
            teacherDtoValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<CreateTeacherDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var mapper = new MapperConfiguration(
                cfg => cfg.AddProfile<UsersProfile>())
                .CreateMapper();

            this.authenticationService = new AuthenticationService(
                this.studentRepository,
                this.teacherRepository,
                configurationMock.Object,
                mapper,
                studentDtoValidatorMock.Object,
                teacherDtoValidatorMock.Object);
        }

        [Test]
        public async Task RegisterStudent_WithValidData_ShouldReturnSuccess()
        {
            this.studentRepository.Entities.Clear();
            var student = new CreateStudentDto
            {
                Email = "email",
                Password = "password",
                Name = "name",
                Surname = "surname",
                CustomId = "customId",
            };

            var result = await this.authenticationService.RegisterStudentAsync(student);
            Assert.That(result.IsT0);
        }

        [Test]
        public async Task RegisterStudent_WithValidData_ShouldAddStudent()
        {
            this.studentRepository.Entities.Clear();
            var student = new CreateStudentDto
            {
                Email = "email",
                Password = "password",
                Name = "name",
                Surname = "surname",
                CustomId = "customId",
            };

            await this.authenticationService.RegisterStudentAsync(student);
            var dbStudent = this.studentRepository.GetAll().FirstOrDefault();

            Assert.That(dbStudent, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(dbStudent.Email, Is.EqualTo(student.Email));
                Assert.That(dbStudent.Name, Is.EqualTo(student.Name));
                Assert.That(dbStudent.Surname, Is.EqualTo(student.Surname));
                Assert.That(dbStudent.CustomId, Is.EqualTo(student.CustomId));
            });
        }

        [Test]
        public async Task RegisterStudent_WithExistingEmail_ShouldReturnUserAlreadyExists()
        {
            this.studentRepository.Entities.Clear();
            var student = new CreateStudentDto
            {
                Email = "email",
                Password = "password",
                Name = "name",
                Surname = "surname",
                CustomId = "customId",
            };

            await this.authenticationService.RegisterStudentAsync(student);
            var result = await this.authenticationService.RegisterStudentAsync(student);
            Assert.That(result.IsT2);
        }

        [Test]
        public async Task RegisterStudent_WithExistingEmail_ShouldNotAddStudent()
        {
            this.studentRepository.Entities.Clear();
            var student = new CreateStudentDto
            {
                Email = "email",
                Password = "password",
                Name = "name",
                Surname = "surname",
                CustomId = "customId",
            };

            await this.authenticationService.RegisterStudentAsync(student);
            await this.authenticationService.RegisterStudentAsync(student);

            var studentsCount = this.studentRepository.GetAll().Count();
            Assert.That(studentsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task LoginStudent_WithValidData_ShouldReturnToken()
        {
            this.studentRepository.Entities.Clear();
            this.studentRepository.Entities.Add(
                new Student
                {
                    Email = "email",
                    PasswordHash = "$2a$11$ndBpDe1qzTp80UOmMvQFq.tWKebXIh2ghn4/z2H839iwW58zEaCgO",
                    Name = "name",
                    Surname = "surname",
                    CustomId = "customId"
                });

            var student = new UserAuthDto
            {
                Email = "email",
                Password = "password",
            };

            var result = await this.authenticationService.LoginStudentAsync(student);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0, Is.Not.Null.And.Not.Empty);
            });
        }

        [Test]
        public async Task LoginStudent_WithInvalidData_ShouldReturnNotFound()
        {
            this.studentRepository.Entities.Clear();
            var student = new UserAuthDto
            {
                Email = "email",
                Password = "password",
            };

            var result = await this.authenticationService.LoginStudentAsync(student);
            Assert.That(result.IsT1);
        }

        [Test]
        public async Task RegisterTeacher_WithValidData_ShouldReturnSuccess()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new CreateTeacherDto
            {
                Email = "email",
                Password = "password",
                Name = "name",
                Surname = "surname",
            };

            var result = await this.authenticationService.RegisterTeacherAsync(teacher);
            Assert.That(result.IsT0);
        }

        [Test]
        public async Task RegisterTeacher_WithValidData_ShouldAddTeacher()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new CreateTeacherDto
            {
                Email = "email",
                Password = "password",
                Name = "name",
                Surname = "surname",
            };

            await this.authenticationService.RegisterTeacherAsync(teacher);
            var dbTeacher = this.teacherRepository.GetAll().FirstOrDefault();
            Assert.That(dbTeacher, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(dbTeacher.Email, Is.EqualTo(teacher.Email));
                Assert.That(dbTeacher.Name, Is.EqualTo(teacher.Name));
                Assert.That(dbTeacher.Surname, Is.EqualTo(teacher.Surname));
            });
        }

        [Test]
        public async Task RegisterTeacher_WithExistingEmail_ShouldReturnUserAlreadyExists()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new CreateTeacherDto
            {
                Email = "email",
                Password = "password",
                Name = "name",
                Surname = "surname",
            };

            await this.authenticationService.RegisterTeacherAsync(teacher);
            var result = await this.authenticationService.RegisterTeacherAsync(teacher);
            Assert.That(result.IsT2);
        }

        [Test]
        public async Task RegisterTeacher_WithExistingEmail_ShouldNotAddTeacher()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new CreateTeacherDto
            {
                Email = "email",
                Password = "password",
                Name = "name",
                Surname = "surname",
            };

            await this.authenticationService.RegisterTeacherAsync(teacher);
            await this.authenticationService.RegisterTeacherAsync(teacher);

            var teachersCount = this.teacherRepository.GetAll().Count();
            Assert.That(teachersCount, Is.EqualTo(1));
        }

        [Test]
        public async Task LoginTeacher_WithValidData_ShouldReturnToken()
        {
            this.teacherRepository.Entities.Clear();
            this.teacherRepository.Entities.Add(
                new Teacher
                {
                    Email = "email",
                    PasswordHash = "$2a$11$ndBpDe1qzTp80UOmMvQFq.tWKebXIh2ghn4/z2H839iwW58zEaCgO",
                    Name = "name",
                    Surname = "surname",
                });

            var teacher = new UserAuthDto
            {
                Email = "email",
                Password = "password",
            };

            var result = await this.authenticationService.LoginTeacherAsync(teacher);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0, Is.Not.Null.And.Not.Empty);
            });
        }

        [Test]
        public async Task LoginTeacher_WithInvalidData_ShouldReturnNotFound()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new UserAuthDto
            {
                Email = "email",
                Password = "password",
            };

            var result = await this.authenticationService.LoginTeacherAsync(teacher);
            Assert.That(result.IsT1);
        }
    }
}

using Moq;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Application.Tests.Fakes;
using Graidex.Application.Interfaces;
using FluentValidation;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.DTOs.Authentication;
using AutoMapper;
using Graidex.Application.Services.Users.Teachers;
using Graidex.Application.AutoMapperProfiles;
using Graidex.Domain.Models.Users;
using System.ComponentModel.DataAnnotations;
using Graidex.Application.DTOs.Subject;

namespace Graidex.Application.Tests.Services
{
    internal class TeacherServiceTests
    {
        private FakeStudentRepository studentRepository;
        private FakeTeacherRepository teacherRepository;
        private FakeSubjectRepository subjectRepository;
        private TeacherService teacherService;

        [SetUp]
        public void Setup()
        {
            this.studentRepository = new FakeStudentRepository();
            this.teacherRepository = new FakeTeacherRepository();
            this.subjectRepository = new FakeSubjectRepository();

            var currentUserMock = new Mock<ICurrentUserService>();
            currentUserMock
                .Setup(a => a.GetEmail())
                .Returns("email");

            var mapper = new MapperConfiguration(
                cfg => cfg.AddProfile<UsersProfile>())
                .CreateMapper();

            var teacherInfoDtoValidatorMock = new Mock<IValidator<TeacherInfoDto>>();
            teacherInfoDtoValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<TeacherInfoDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var changePasswordDtoValidatorMock = new Mock<IValidator<ChangePasswordDto>>();
            changePasswordDtoValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<ChangePasswordDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            this.teacherService = new TeacherService(
                currentUserMock.Object,
                this.studentRepository,
                this.teacherRepository,
                this.subjectRepository,
                mapper,
                teacherInfoDtoValidatorMock.Object,
                changePasswordDtoValidatorMock.Object
                );
        }

        #region DeleteCurrent
        [Test]
        public async Task DeleteCurrent_WithValidData_ShouldReturnSuccess()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var result = await this.teacherService.DeleteCurrent("password");

            Assert.That(result.IsT0);
        }

        [Test]
        public async Task DeleteCurrent_WithValidData_ShouldDeleteCurrentTeacher()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            await this.teacherService.DeleteCurrent("password");

            Assert.That(this.teacherRepository.GetByEmail("email").Result, Is.Null);
        }

        [Test]
        public async Task DeleteCurrent_WithInvalidEmail_ShouldReturnNotFound()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var result = await this.teacherService.DeleteCurrent("password");

            Assert.That(result.IsT1);
        }

        [Test]
        public async Task DeleteCurrent_WithInvalidPassword_ShouldReturnWrongPassword()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var result = await this.teacherService.DeleteCurrent("wrongpassword");

            Assert.That(result.IsT2);
        }
        #endregion DeleteCurrent

        #region GetByEmail
        [Test]
        public async Task GetByEmail_WithValidData_ShouldReturnTeacherInfoDto()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var result = await this.teacherService.GetByEmailAsync("email");

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0.Name, Is.EqualTo("name"));
                Assert.That(result.AsT0.Surname, Is.EqualTo("surname"));
            });
        }

        [Test]
        public async Task GetByEmail_WithNonexistentTeacher_ShouldReturnUserNotFound()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var result = await this.teacherService.GetByEmailAsync("email");

            Assert.That(result.IsT1);
        }
        #endregion GetByEmail

        #region GetCurrent
        [Test]
        public async Task GetCurrent_WithValidData_ShouldReturnTeacherInfoDto()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var result = await this.teacherService.GetCurrentAsync();

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0.Name, Is.EqualTo("name"));
                Assert.That(result.AsT0.Surname, Is.EqualTo("surname"));
            });
        }

        [Test]
        public async Task GetCurrent_WithInvalidData_ShouldReturnUserNotFound()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var result = await this.teacherService.GetCurrentAsync();

            Assert.That(result.IsT1);
        }
        #endregion GetCurrent

        #region UpdateCurrentInfo
        [Test]
        public async Task UpdateCurrentInfo_WithValidData_ShouldReturnSuccess()
        {

            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var updateDto = new TeacherInfoDto
            {
                Name = "newname",
                Surname = "newsurname"
            };

            var result = await this.teacherService.UpdateCurrentInfoAsync(updateDto);

            Assert.That(result.IsT0);
        }

        [Test]
        public async Task UpdateCurrentInfo_WithValidData_ShouldUpdateInfo()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var updateDto = new TeacherInfoDto
            {
                Name = "newname",
                Surname = "newsurname"
            };

            await this.teacherService.UpdateCurrentInfoAsync(updateDto);

            var dbTeacher = this.teacherRepository.GetByEmail("email").Result;
            Assert.That(dbTeacher, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(dbTeacher.Name, Is.EqualTo("newname"));
                Assert.That(dbTeacher.Surname, Is.EqualTo("newsurname"));
            });
        }

        [Test]
        public async Task UpdateCurrentInfo_WithInvalidData_ShouldReturnNotFound()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var updateDto = new TeacherInfoDto
            {
                Name = "newname",
                Surname = "newsurname"
            };

            var result = await this.teacherService.UpdateCurrentInfoAsync(updateDto);

            Assert.That(result.IsT2);
        }
        #endregion UpdateCurrentInfo

        #region UpdateCurrentPassword
        [Test]
        public async Task UpdateCurrentPassword_WithValidData_ShouldReturnSuccess()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var updatePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = "password",
                NewPassword = "newpassword"
            };

            var result = await this.teacherService.UpdateCurrentPasswordAsync(updatePasswordDto);

            Assert.That(result.IsT0);
        }

        [Test]
        public async Task UpdateCurrentPassword_WithInvalidData_ShouldReturnNotFound()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var updatePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = "password",
                NewPassword = "newpassword"
            };

            var result = await this.teacherService.UpdateCurrentPasswordAsync(updatePasswordDto);

            Assert.That(result.IsT2);
        }

        [Test]
        public async Task UpdateCurrentPassword_WithInvalidPassword_ShouldReturnWrongPassword()
        {
            this.teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var updatePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = "wrongpassword",
                NewPassword = "newpassword"
            };

            var result = await this.teacherService.UpdateCurrentPasswordAsync(updatePasswordDto);

            Assert.That(result.IsT3);
        }
        #endregion UpdateCurrentPassword
    }
}

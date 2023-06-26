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

namespace Graidex.Application.Tests.Services.Users
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
            studentRepository = new FakeStudentRepository();
            teacherRepository = new FakeTeacherRepository();
            subjectRepository = new FakeSubjectRepository();

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

            teacherService = new TeacherService(
                currentUserMock.Object,
                studentRepository,
                teacherRepository,
                subjectRepository,
                mapper,
                teacherInfoDtoValidatorMock.Object,
                changePasswordDtoValidatorMock.Object
                );
        }

        #region DeleteCurrent
        [Test]
        public async Task DeleteCurrent_WithValidData_ShouldReturnSuccess()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var result = await teacherService.DeleteCurrent("password");

            Assert.That(result.IsT0);
        }

        [Test]
        public async Task DeleteCurrent_WithValidData_ShouldDeleteCurrentTeacher()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            await teacherService.DeleteCurrent("password");

            Assert.That(teacherRepository.GetByEmail("email").Result, Is.Null);
        }

        [Test]
        public async Task DeleteCurrent_WithInvalidEmail_ShouldReturnNotFound()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var result = await teacherService.DeleteCurrent("password");

            Assert.That(result.IsT1);
        }

        [Test]
        public async Task DeleteCurrent_WithInvalidPassword_ShouldReturnWrongPassword()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var result = await teacherService.DeleteCurrent("wrongpassword");

            Assert.That(result.IsT2);
        }
        #endregion DeleteCurrent

        #region GetByEmail
        [Test]
        public async Task GetByEmail_WithValidData_ShouldReturnTeacherInfoDto()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var result = await teacherService.GetByEmailAsync("email");

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
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var result = await teacherService.GetByEmailAsync("email");

            Assert.That(result.IsT1);
        }
        #endregion GetByEmail

        #region GetCurrent
        [Test]
        public async Task GetCurrent_WithValidData_ShouldReturnTeacherInfoDto()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var result = await teacherService.GetCurrentAsync();

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
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var result = await teacherService.GetCurrentAsync();

            Assert.That(result.IsT1);
        }
        #endregion GetCurrent

        #region UpdateCurrentInfo
        [Test]
        public async Task UpdateCurrentInfo_WithValidData_ShouldReturnSuccess()
        {

            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var updateDto = new TeacherInfoDto
            {
                Name = "newname",
                Surname = "newsurname"
            };

            var result = await teacherService.UpdateCurrentInfoAsync(updateDto);

            Assert.That(result.IsT0);
        }

        [Test]
        public async Task UpdateCurrentInfo_WithValidData_ShouldUpdateInfo()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var updateDto = new TeacherInfoDto
            {
                Name = "newname",
                Surname = "newsurname"
            };

            await teacherService.UpdateCurrentInfoAsync(updateDto);

            var dbTeacher = teacherRepository.GetByEmail("email").Result;
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
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var updateDto = new TeacherInfoDto
            {
                Name = "newname",
                Surname = "newsurname"
            };

            var result = await teacherService.UpdateCurrentInfoAsync(updateDto);

            Assert.That(result.IsT2);
        }
        #endregion UpdateCurrentInfo

        #region UpdateCurrentPassword
        [Test]
        public async Task UpdateCurrentPassword_WithValidData_ShouldReturnSuccess()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var updatePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = "password",
                NewPassword = "newpassword"
            };

            var result = await teacherService.UpdateCurrentPasswordAsync(updatePasswordDto);

            Assert.That(result.IsT0);
        }

        [Test]
        public async Task UpdateCurrentPassword_WithInvalidData_ShouldReturnNotFound()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var updatePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = "password",
                NewPassword = "newpassword"
            };

            var result = await teacherService.UpdateCurrentPasswordAsync(updatePasswordDto);

            Assert.That(result.IsT2);
        }

        [Test]
        public async Task UpdateCurrentPassword_WithInvalidPassword_ShouldReturnWrongPassword()
        {
            teacherRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            teacherRepository.Entities.Add(teacher);

            var updatePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = "wrongpassword",
                NewPassword = "newpassword"
            };

            var result = await teacherService.UpdateCurrentPasswordAsync(updatePasswordDto);

            Assert.That(result.IsT3);
        }
        #endregion UpdateCurrentPassword
    }
}

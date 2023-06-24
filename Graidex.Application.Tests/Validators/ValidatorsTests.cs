using FluentValidation;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.Validators.Authentication;
using Graidex.Application.Validators.Users.Students;
using Graidex.Application.Validators.Users.Teachers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Validators
{
    internal class ValidatorsTests
    {
        private ChangePasswordDtoValidator changePasswordDtoValidator;
        private CreateStudentDtoValidator createStudentDtoValidator;
        private StudentInfoDtoValidator studentInfoDtoValidator;
        private CreateTeacherDtoValidator createTeacherDtoValidator;
        private TeacherInfoDtoValidator teacherInfoDtoValidator;
        [SetUp]
        public void SetUp()
        {
            this.changePasswordDtoValidator = new ChangePasswordDtoValidator();
            this.createStudentDtoValidator = new CreateStudentDtoValidator();
            this.studentInfoDtoValidator = new StudentInfoDtoValidator();
            this.createTeacherDtoValidator = new CreateTeacherDtoValidator();
            this.teacherInfoDtoValidator = new TeacherInfoDtoValidator();
        }

        #region ValidateChangePasswordDto
        [Test]
        public async Task ValidateChangePasswordDto_WithValidData_ShouldReturnSuccess()
        {
            var changePasswordDto = new ChangePasswordDto
            {   CurrentPassword = "password",
                NewPassword = "!1Password"
            };

            var result = await this.changePasswordDtoValidator.ValidateAsync(changePasswordDto);

            Assert.That(result.IsValid);
        }

        [TestCase("", "'New Password' must not be empty.")]
        [TestCase("_", "Password must contain at least one lowercase letter.")]
        [TestCase("pass", "Password length must be from 8 to 16 symbols")]
        [TestCase("password", "'NewPassword' should not be equal to 'CurrentPassword'")]
        [TestCase("pasуword", "Password must contain at least one number.")]
        [TestCase("1password", "Password must contain at least one of [!?*.$].")]
        [TestCase("1!password", "Password must contain at least one uppercase letter.")]
        public async Task ValidateChangePasswordDto_WithInvalidData_ShouldReturnFailures(string password, string errorMessage)
        {
            var changePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = "password",
                NewPassword = password
            };

            var result = await this.changePasswordDtoValidator.ValidateAsync(changePasswordDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }
        #endregion ValidateChangePasswordDto

        #region ValidateCreateStudentDto
        [Test]
        public async Task ValidateCreateStudentDto_WithValidData_ShouldReturnSuccess()
        {
            var createStudentDto = new CreateStudentDto  
            {
                Email = "email@email.com",
                Password = "!1Password",
                Name = "na me-na",
                Surname = "surname",
                CustomId = "customId"
            };

            var result = await this.createStudentDtoValidator.ValidateAsync(createStudentDto);

            Assert.That(result.IsValid);
        }

        [TestCase(" ", "'Email' must not be empty.")]
        [TestCase("email", "'Email' is not a valid email address.")]
        public async Task ValidateCreateStudentDto_WithInvalidEmail_ShouldReturnFailures(string email, string errorMessage)
        {
            var createStudentDto = new CreateStudentDto
            {
                Email = email,
                Password = "!1Password",
                Name = "name",
                Surname = "surname",
                CustomId = "customId"
            };

            var result = await this.createStudentDtoValidator.ValidateAsync(createStudentDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("", "'Password' must not be empty.")]
        [TestCase("_", "Password must contain at least one lowercase letter.")]
        [TestCase("pass", "Password length must be from 8 to 16 symbols")]
        [TestCase("pasуword", "Password must contain at least one number.")]
        [TestCase("1password", "Password must contain at least one of [!?*.$].")]
        [TestCase("1!password", "Password must contain at least one uppercase letter.")]
        public async Task ValidateCreateStudentDto_WithInvalidPassword_ShouldReturnFailures(string password, string errorMessage)
        {
            var createStudentDto = new CreateStudentDto
            {
                Email = "email@email.com",
                Password = password,
                Name = "name",
                Surname = "surname",
                CustomId = "customId"
            };

            var result = await this.createStudentDtoValidator.ValidateAsync(createStudentDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("", "'Name' must not be empty.")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "'Name' must be between 1 and 50 characters. You entered 51 characters.")]
        [TestCase("na_me", "This field must contain only letters, dashes or space symbols.")]
        [TestCase("- -", "This field must contain at least one letter.")]
        public async Task ValidateCreateStudentDto_WithInvalidName_ShouldReturnFailures(string name, string errorMessage)
        {
            var createStudentDto = new CreateStudentDto
            {
                Email = "email@email.com",
                Password = "!1Password",
                Name = name,
                Surname = "surname",
                CustomId = "customId"
            };

            var result = await this.createStudentDtoValidator.ValidateAsync(createStudentDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("", "'Surname' must not be empty.")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "'Surname' must be between 1 and 50 characters. You entered 51 characters.")]
        [TestCase("surna_me", "This field must contain only letters, dashes or space symbols.")]
        [TestCase("- -", "This field must contain at least one letter.")]
        public async Task ValidateCreateStudentDto_WithInvalidSurname_ShouldReturnFailures(string surname, string errorMessage)
        {
            var createStudentDto = new CreateStudentDto
            {
                Email = "email@email.com",
                Password = "!1Password",
                Name = "name",
                Surname = surname,
                CustomId = "customId"
            };

            var result = await this.createStudentDtoValidator.ValidateAsync(createStudentDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("aaaaaaaaaaaaaaaa", "'Custom Id' must be between 0 and 15 characters. You entered 16 characters.")]
        public async Task ValidateCreateStudentDto_WithInvalidCustomId_ShouldReturnFailures(string customId, string errorMessage)
        {
            var createStudentDto = new CreateStudentDto
            {
                Email = "email@email.com",
                Password = "!1Password",
                Name = "name",
                Surname = "surname",
                CustomId = customId
            };

            var result = await this.createStudentDtoValidator.ValidateAsync(createStudentDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }
        #endregion ValidateCreateStudentDto

        #region ValidateStudentInfoDto
        [Test]
        public async Task ValidateStudentInfoDto_WithValidData_ShouldReturnSuccess()
        {
            var studentInfoDto = new StudentInfoDto
            {
                Name = "na me-na",
                Surname = "surname",
                CustomId = "customId"
            };

            var result = await this.studentInfoDtoValidator.ValidateAsync(studentInfoDto);

            Assert.That(result.IsValid);
        }

        [TestCase("", "'Name' must not be empty.")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "'Name' must be between 1 and 50 characters. You entered 51 characters.")]
        [TestCase("na_me", "This field must contain only letters, dashes or space symbols.")]
        [TestCase("- -", "This field must contain at least one letter.")]
        public async Task ValidateStudentInfoDto_WithInvalidName_ShouldReturnFailures(string name, string errorMessage)
        {
            var studentInfoDto = new StudentInfoDto
            {
                Name = name,
                Surname = "surname",
                CustomId = "customId"
            };

            var result = await this.studentInfoDtoValidator.ValidateAsync(studentInfoDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("", "'Surname' must not be empty.")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "'Surname' must be between 1 and 50 characters. You entered 51 characters.")]
        [TestCase("surna_me", "This field must contain only letters, dashes or space symbols.")]
        [TestCase("- -", "This field must contain at least one letter.")]
        public async Task ValidateStudentInfoDto_WithInvalidSurname_ShouldReturnFailures(string surname, string errorMessage)
        {
            var studentInfoDto = new StudentInfoDto
            {
                Name = "name",
                Surname = surname,
                CustomId = "customId"
            };

            var result = await this.studentInfoDtoValidator.ValidateAsync(studentInfoDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("aaaaaaaaaaaaaaaa", "'Custom Id' must be between 0 and 15 characters. You entered 16 characters.")]
        public async Task ValidateStudentInfoDto_WithInvalidCustomId_ShouldReturnFailures(string customId, string errorMessage)
        {
            var studentInfoDto = new StudentInfoDto
            {
                Name = "name",
                Surname = "surname",
                CustomId = customId
            };

            var result = await this.studentInfoDtoValidator.ValidateAsync(studentInfoDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }
        #endregion ValidateStudentInfoDto

        #region ValidateCreateTeacherDto
        [Test]
        public async Task ValidateCreateTeacherDto_WithValidData_ShouldReturnSuccess()
        {
            var createTeacherDto = new CreateTeacherDto
            {
                Email = "email@email.com",
                Password = "!1Password",
                Name = "na me-na",
                Surname = "surname",
            };

            var result = await this.createTeacherDtoValidator.ValidateAsync(createTeacherDto);

            Assert.That(result.IsValid);
        }

        [TestCase(" ", "'Email' must not be empty.")]
        [TestCase("email", "'Email' is not a valid email address.")]
        public async Task ValidateCreateTeacherDto_WithInvalidEmail_ShouldReturnFailures(string email, string errorMessage)
        {
            var createTeacherDto = new CreateTeacherDto
            {
                Email = email,
                Password = "!1Password",
                Name = "name",
                Surname = "surname",
            };

            var result = await this.createTeacherDtoValidator.ValidateAsync(createTeacherDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("", "'Password' must not be empty.")]
        [TestCase("_", "Password must contain at least one lowercase letter.")]
        [TestCase("pass", "Password length must be from 8 to 16 symbols")]
        [TestCase("pasуword", "Password must contain at least one number.")]
        [TestCase("1password", "Password must contain at least one of [!?*.$].")]
        [TestCase("1!password", "Password must contain at least one uppercase letter.")]
        public async Task ValidateCreateTeacherDto_WithInvalidPassword_ShouldReturnFailures(string password, string errorMessage)
        {
            var createTeacherDto = new CreateTeacherDto
            {
                Email = "email@email.com",
                Password = password,
                Name = "name",
                Surname = "surname",
            };

            var result = await this.createTeacherDtoValidator.ValidateAsync(createTeacherDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("", "'Name' must not be empty.")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "'Name' must be between 1 and 50 characters. You entered 51 characters.")]
        [TestCase("na_me", "This field must contain only letters, dashes or space symbols.")]
        [TestCase("- -", "This field must contain at least one letter.")]
        public async Task ValidateCreateTeacherDto_WithInvalidName_ShouldReturnFailures(string name, string errorMessage)
        {
            var createTeacherDto = new CreateTeacherDto
            {
                Email = "email@email.com",
                Password = "!1Password",
                Name = name,
                Surname = "surname",
            };

            var result = await this.createTeacherDtoValidator.ValidateAsync(createTeacherDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("", "'Surname' must not be empty.")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "'Surname' must be between 1 and 50 characters. You entered 51 characters.")]
        [TestCase("surna_me", "This field must contain only letters, dashes or space symbols.")]
        [TestCase("- -", "This field must contain at least one letter.")]
        public async Task ValidateCreateTeacherDto_WithInvalidSurname_ShouldReturnFailures(string surname, string errorMessage)
        {
            var createTeacherDto = new CreateTeacherDto
            {
                Email = "email@email.com",
                Password = "!1Password",
                Name = "name",
                Surname = surname,
            };

            var result = await this.createTeacherDtoValidator.ValidateAsync(createTeacherDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }
        #endregion ValidateCreateTeacherDto

        #region ValidateTeacherInfoDto
        [Test]
        public async Task ValidateTeacherInfoDto_WithValidData_ShouldReturnSuccess()
        {
            var teacherInfoDto = new TeacherInfoDto
            {
                Name = "na me-na",
                Surname = "surname",
            };

            var result = await this.teacherInfoDtoValidator.ValidateAsync(teacherInfoDto);

            Assert.That(result.IsValid);
        }

        [TestCase("", "'Name' must not be empty.")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "'Name' must be between 1 and 50 characters. You entered 51 characters.")]
        [TestCase("na_me", "This field must contain only letters, dashes or space symbols.")]
        [TestCase("- -", "This field must contain at least one letter.")]
        public async Task ValidateTeacherInfoDto_WithInvalidName_ShouldReturnFailures(string name, string errorMessage)
        {
            var teacherInfoDto = new TeacherInfoDto
            {
                Name = name,
                Surname = "surname",
            };

            var result = await this.teacherInfoDtoValidator.ValidateAsync(teacherInfoDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }

        [TestCase("", "'Surname' must not be empty.")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "'Surname' must be between 1 and 50 characters. You entered 51 characters.")]
        [TestCase("surna_me", "This field must contain only letters, dashes or space symbols.")]
        [TestCase("- -", "This field must contain at least one letter.")]
        public async Task ValidateTeacherInfoDto_WithInvalidSurname_ShouldReturnFailures(string surname, string errorMessage)
        {
            var teacherInfoDto = new TeacherInfoDto
            {
                Name = "name",
                Surname = surname,
            };

            var result = await this.teacherInfoDtoValidator.ValidateAsync(teacherInfoDto);

            Assert.That(result.Errors.FirstOrDefault(x => x.ErrorMessage == errorMessage), Is.Not.Null);
        }
        #endregion ValidateTeacherInfoDto
    }
}

using AutoMapper;
using FluentValidation;
using Graidex.Application.AutoMapperProfiles;
using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Files;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.DTOs.Users.Teachers;
using Graidex.Application.Interfaces;
using Graidex.Application.Services.Users.Students;
using Graidex.Application.Tests.Fakes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Services.Users
{
    internal class StudentServiceTests
    {
        private StudentService studentService;
        private FakeStudentRepository studentRepository;
        private FakeTeacherRepository teacherRepository;
        private FakeSubjectRepository subjectRepository;
        private Mock<ICurrentUserService> currentUserMock;

        [SetUp]
        public void Setup()
        {
            this.studentRepository = new();
            this.teacherRepository = new();
            this.subjectRepository = new();

            this.currentUserMock = new Mock<ICurrentUserService>();

            var mapper = new MapperConfiguration(
                cfg => cfg.AddProfile<UsersProfile>())
                .CreateMapper();

            var studentInfoDtoValidator = new Mock<IValidator<StudentInfoDto>>();
            studentInfoDtoValidator
                .Setup(x => x.ValidateAsync(It.IsAny<StudentInfoDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var changePasswordDtoValidator = new Mock<IValidator<ChangePasswordDto>>();
            changePasswordDtoValidator
                .Setup(x => x.ValidateAsync(It.IsAny<ChangePasswordDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var uploadImageDtoValidator = new Mock<IValidator<UploadImageDto>>();
            uploadImageDtoValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UploadImageDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var fileStorageMock = new Mock<IFileStorageProvider>();

            this.studentService = new StudentService(
                currentUserMock.Object,
                studentRepository,
                teacherRepository,
                subjectRepository,
                mapper,
                studentInfoDtoValidator.Object,
                changePasswordDtoValidator.Object,
                uploadImageDtoValidator.Object,
                fileStorageMock.Object);
        }

        #region GetByEmailAsync

        [Test]
        public async Task GetByEmailAsync_WhenStudentExists_ReturnsStudentInfoDto()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = email,
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<pass-hash>",
            };

            this.studentRepository.Entities.Add(student);

            var result = await this.studentService.GetByEmailAsync(email);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0.Name, Is.EqualTo(student.Name));
                Assert.That(result.AsT0.Surname, Is.EqualTo(student.Surname));
                Assert.That(result.AsT0.CustomId, Is.EqualTo(student.CustomId));
            });
        }

        [TestCase("NotExistedStudentEmail@somewhere.com")]
        [TestCase("")]
        public async Task GetByEmailAsync_WhenStudentDoesNotExist_ReturnsUserNotFound(string email)
        {
            this.studentRepository.Entities.Clear();

            var result = await this.studentService.GetByEmailAsync(email);

            Assert.That(result.IsT1);
        }

        #endregion

        #region GetCurrentAsync

        [Test]
        public async Task GetCurrentAsync_WhenStudentExists_ReturnsStudentInfoDto()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = email,
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<pass-hash>",
            };

            this.studentRepository.Entities.Add(student);

            var result = await this.studentService.GetCurrentAsync();

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0.Name, Is.EqualTo(student.Name));
                Assert.That(result.AsT0.Surname, Is.EqualTo(student.Surname));
                Assert.That(result.AsT0.CustomId, Is.EqualTo(student.CustomId));
            });
        }

        [Test]
        public async Task GetCurrentAsync_WhenStudentDoesNotExist_ReturnsUserNotFound()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();

            var result = await this.studentService.GetCurrentAsync();

            Assert.That(result.IsT1);
        }

        #endregion

        #region UpdateCurrentInfoAsync

        [Test]
        public async Task UpdateCurrentInfoAsync_WhenStudentExists_StudentInfoUpdated()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = email,
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<pass-hash>",
            };

            this.studentRepository.Entities.Add(student);

            var result = await this.studentService.UpdateCurrentInfoAsync(new StudentInfoDto
            {
                Name = "NewName",
                Surname = "NewSurname",
                CustomId = "NewCustomId",
            });

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(student.Name, Is.EqualTo("NewName"));
                Assert.That(student.Surname, Is.EqualTo("NewSurname"));
                Assert.That(student.CustomId, Is.EqualTo("NewCustomId"));
            });
        }

        [Test]
        public async Task UpdateCurrentInfoAsync_WhenStudentDoesNotExist_ReturnsUserNotFound()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();

            var result = await this.studentService.UpdateCurrentInfoAsync(new StudentInfoDto
            {
                Name = "NewName",
                Surname = "NewSurname",
                CustomId = "NewCustomId",
            });

            Assert.That(result.IsT2);
        }

        #endregion

        #region UpdateCurrentPasswordAsync
        [Test]
        public async Task UpdateCurrentPasswordAsync_WhenStudentExists_PasswordUpdated()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = email,
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "$2a$11$b6HK0vZCeSuhZaYn1J/xo.JD1qhDHOcrVilrdfl52k8wvqajFgbgO",
            };

            this.studentRepository.Entities.Add(student);

            var result = await this.studentService.UpdateCurrentPasswordAsync(new ChangePasswordDto
            {
                CurrentPassword = "OldPassword123!",
                NewPassword = "NewPassword123!",
            });

            Assert.That(result.IsT0);
        }

        [Test]
        public async Task UpdateCurrentPasswordAsync_WhenCurrentPasswordIsWrong_ReturnsWrongPassword()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = email,
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "$2a$11$b6HK0vZCeSuhZaYn1J/xo.JD1qhDHOcrVilrdfl52k8wvqajFgbgO",
            };

            this.studentRepository.Entities.Add(student);

            var result = await this.studentService.UpdateCurrentPasswordAsync(new ChangePasswordDto
            {
                CurrentPassword = "WrongOldPassword123!",
                NewPassword = "NewPassword123!",
            });

            Assert.That(result.IsT3);
        }

        [Test]
        public async Task UpdateCurrentPasswordAsync_WhenStudentDoesNotExist_ReturnsUserNotFound()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();

            var result = await this.studentService.UpdateCurrentPasswordAsync(new ChangePasswordDto
            {
                CurrentPassword = "OldPassword123!",
                NewPassword = "NewPassword123!",
            });

            Assert.That(result.IsT2);
        }

        #endregion

        #region DeleteCurrentAsync

        [Test]
        public async Task DeleteCurrentAsync_WhenStudentExists_StudentDeleted()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = email,
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "$2a$11$b6HK0vZCeSuhZaYn1J/xo.JD1qhDHOcrVilrdfl52k8wvqajFgbgO",
            };

            this.studentRepository.Entities.Add(student);

            var result = await this.studentService.DeleteCurrentAsync("OldPassword123!");

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(this.studentRepository.Entities.Any(), Is.False);
            });
        }

        [Test]
        public async Task DeleteCurrentAsync_WhenStudentDoesNotExist_ReturnsUserNotFound()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();
            var result = await this.studentService.DeleteCurrentAsync("OldPassword123!");

            Assert.That(result.IsT1);
        }

        [Test]
        public async Task DeleteCurrentAsync_WhenPasswordIsWrong_ReturnsWrongPassword()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(x => x.GetEmail())
                .Returns(email);

            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = email,
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "$2a$11$b6HK0vZCeSuhZaYn1J/xo.JD1qhDHOcrVilrdfl52k8wvqajFgbgO",
            };

            this.studentRepository.Entities.Add(student);

            var result = await this.studentService.DeleteCurrentAsync("WrongPassword123!");

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT2);
                Assert.That(this.studentRepository.Entities.Contains(student));
            });
        }


        #endregion

        #region AddToSubjectAsync

        [Test]
        public async Task AddToSubjectAsync_WhenStudentExistsAndSubjectExists_StudentAddedToSubject()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = email,
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<password-hash>",
            };

            this.studentRepository.Entities.Add(student);

            this.subjectRepository.Entities.Clear();
            var subject = new Subject
            {
                Id = 1,
                Title = "SubjectTitle",
                CustomId = "SubjectCustomId",
                TeacherId = 0,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.studentService.AddToSubjectAsync(subject.Id, email);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(this.subjectRepository.Entities[0].Students.Contains(student));
            });
        }

        [Test]
        public async Task AddToSubjectAsync_WhenStudentDoesNotExist_ReturnsUserNotFound()
        {

            this.studentRepository.Entities.Clear();

            this.subjectRepository.Entities.Clear();
            var subject = new Subject
            {
                Id = 1,
                Title = "SubjectTitle",
                CustomId = "SubjectCustomId",
                TeacherId = 0,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.studentService.AddToSubjectAsync(
                subject.Id, 
                "NotExistedStudentEmail@somewhere.com");

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT1);
                Assert.That(this.subjectRepository.Entities[0].Students.Any(), Is.False);
            });
        }

        [Test]
        public async Task AddToSubjectAsync_WhenSubjectDoesNotExist_ReturnsNotFound()
        {
            string email = "ExistedStudentEmail@somewhere.com";

            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = email,
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<password-hash>",
            };

            this.studentRepository.Entities.Add(student);

            this.subjectRepository.Entities.Clear();

            var result = await this.studentService.AddToSubjectAsync(1, email);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT2);
                Assert.That(this.subjectRepository.Entities.Any(), Is.False);
            });
        }

        #endregion

        #region GetAllOfSubjectAsync

        [Test]
        public async Task GetAllOfSubjectAsync_WhenSubjectExists_ReturnsAllStudentsOfSubject()
        {
            this.studentRepository.Entities.Clear();
            var student1 = new Student
            {
                Id = 1,
                Email = "student1@somewhere.com",
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<password-hash>",
            };

            var student2 = new Student
            {
                Id = 2,
                Email = "student2@somewhere.com",
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<password-hash>",
            };
            
            this.studentRepository.Entities.Add(student1);
            this.studentRepository.Entities.Add(student2);

            this.subjectRepository.Entities.Clear();
            var subject = new Subject
            {
                Id = 1,
                Title = "SubjectTitle",
                CustomId = "SubjectCustomId",
                TeacherId = 0,
                Students = new[] { student1, student2 },
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.studentService.GetAllOfSubjectAsync(subject.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0, Has.Count.EqualTo(2));
                Assert.That(result.AsT0[0].Email, Is.EqualTo(student1.Email));
                Assert.That(result.AsT0[1].Email, Is.EqualTo(student2.Email));
            });
        }

        [Test]
        public async Task GetAllOfSubjectAsync_WhenSubjectWithNoStudents_ReturnsEmptyCollection()
        {
            this.studentRepository.Entities.Clear();

            this.subjectRepository.Entities.Clear();
            var subject = new Subject
            {
                Id = 1,
                Title = "SubjectTitle",
                CustomId = "SubjectCustomId",
                TeacherId = 0,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.studentService.GetAllOfSubjectAsync(subject.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0, Is.Empty);
            });
        }

        [Test]
        public async Task GetAllOfSubjectAsync_WhenSubjectDoesNotExist_ReturnsNotFound()
        {
            this.studentRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var result = await this.studentService.GetAllOfSubjectAsync(1);
            Assert.That(result.IsT1);
        }

        #endregion

        #region RemoveFromSubjectAsync

        [Test]
        public async Task RemoveFromSubjectAsync_WhenStudentExistsAndSubjectExists_StudentRemovedFromSubject()
        {
            this.studentRepository.Entities.Clear();
            var student1 = new Student
            {
                Id = 1,
                Email = "student1@somewhere.com",
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<password-hash>",
            };

            var student2 = new Student
            {
                Id = 2,
                Email = "student2@somewhere.com",
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<password-hash>",
            };

            this.studentRepository.Entities.Add(student1);
            this.studentRepository.Entities.Add(student2);

            this.subjectRepository.Entities.Clear();
            var subject = new Subject
            {
                Id = 1,
                Title = "SubjectTitle",
                CustomId = "SubjectCustomId",
                TeacherId = 0,
                Students = new List<Student> { student1, student2 },
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.studentService.RemoveFromSubjectAsync(subject.Id, student1.Email);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(this.subjectRepository.Entities[0].Students, Has.Count.EqualTo(1));
                Assert.That(this.subjectRepository.Entities[0].Students.First().Email,
                    Is.EqualTo(student2.Email));
            });
        }

        [Test]
        public async Task RemoveFromSubjectAsync_WhenStudentDoesNotExist_ReturnsUserNotFound()
        {
            this.studentRepository.Entities.Clear();

            this.subjectRepository.Entities.Clear();
            var subject = new Subject
            {
                Id = 1,
                Title = "SubjectTitle",
                CustomId = "SubjectCustomId",
                TeacherId = 0,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.studentService.RemoveFromSubjectAsync(
                subject.Id,
                "NotExistedStudentEmail@somewhere.com");

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT1);
                Assert.That(this.subjectRepository.Entities[0].Students.Any(), Is.False);
            });
        }

        [Test]
        public async Task RemoveFromSubjectAsync_WhenSubjectDoesNotExist_ReturnsNotFound()
        {
            this.studentRepository.Entities.Clear();
            var student = new Student
            {
                Id = 1,
                Email = "student1@somewhere.com",
                Name = "StudentName",
                Surname = "StudentSurname",
                CustomId = "StudentCustomId",
                PasswordHash = "<password-hash>",
            };

            this.studentRepository.Entities.Add(student);

            this.subjectRepository.Entities.Clear();

            var result = await this.studentService.RemoveFromSubjectAsync(0, student.Email);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT2);
                Assert.That(this.subjectRepository.Entities, Is.Empty);
            });
        }

        #endregion

    }
}

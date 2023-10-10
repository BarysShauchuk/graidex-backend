using AutoMapper;
using FluentValidation;
using Graidex.Application.AutoMapperProfiles;
using Graidex.Application.DTOs.Subject;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.Interfaces;
using Graidex.Application.Services.Subjects;
using Graidex.Application.Tests.Fakes;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Services
{
    internal class SubjectServiceTests
    {
        private FakeStudentRepository studentRepository;
        private FakeTeacherRepository teacherRepository;
        private FakeSubjectRepository subjectRepository;
        private FakeTestRepository testRepository;
        private SubjectService subjectService;
        private Mock<ICurrentUserService> currentUserMock = new Mock <ICurrentUserService>(); 


        [SetUp]
        public void Setup()
        {
            this.studentRepository = new FakeStudentRepository();
            this.teacherRepository = new FakeTeacherRepository();
            this.subjectRepository = new FakeSubjectRepository();
            this.testRepository = new FakeTestRepository();

            this.currentUserMock
                .Setup(a => a.GetEmail())
                .Returns("email");
            this.currentUserMock
                .Setup(a => a.GetRoles())
                .Returns(new string[] { "Teacher", "Student" });

            var mapper = new MapperConfiguration(
                cfg => cfg.AddProfile<SubjectsProfile>())
                .CreateMapper();

            var createSubjectDtoValidator = new Mock<IValidator<CreateSubjectDto>>();
            createSubjectDtoValidator
                .Setup(x => x.ValidateAsync(It.IsAny<CreateSubjectDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var updateSubjectDtoValidator = new Mock<IValidator<UpdateSubjectDto>>();
            updateSubjectDtoValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UpdateSubjectDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            this.subjectService = new SubjectService
                (
                this.currentUserMock.Object,
                this.teacherRepository,
                this.studentRepository,
                this.subjectRepository,
                this.testRepository,
                mapper,
                createSubjectDtoValidator.Object,
                updateSubjectDtoValidator.Object
                );
        }

        #region CreateForCurrent
        [Test]
        public async Task CreateForCurrent_WithValidData_ShouldReturnSubjectDto()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var createSubjectDto = new CreateSubjectDto
            {
                CustomId = "customid",
                Title = "title"
            };

            var result = await this.subjectService.CreateForCurrentAsync(createSubjectDto);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0.CustomId, Is.EqualTo("customid"));
                Assert.That(result.AsT0.Title, Is.EqualTo("title"));
            });
        }

        [Test]
        public async Task CreateForCurrent_WithValidData_ShouldAddSubject()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var createSubjectDto = new CreateSubjectDto
            {
                CustomId = "customid",
                Title = "title"
            };

            var result = await this.subjectService.CreateForCurrentAsync(createSubjectDto);

            var subject = subjectRepository.Entities.FirstOrDefault(x => x.CustomId == "customid");

            Assert.That(subject, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(subject.Id, Is.EqualTo(result.AsT0.Id));
                Assert.That(subject.CustomId, Is.EqualTo("customid"));
                Assert.That(subject.Title, Is.EqualTo("title"));
            });
        }

        [Test]
        public async Task CreateForCurrent_WithInvalidData_ShouldReturnUserNotFound()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var createSubjectDto = new CreateSubjectDto
            {
                CustomId = "customid",
                Title = "title"
            };

            var result = await this.subjectService.CreateForCurrentAsync(createSubjectDto);

            Assert.That(result.IsT2);
        }
        #endregion CreateForCurrent

        #region GetAllOfCurrent
        [TestCase("Teacher")]
        [TestCase("Student")]
        public async Task GetAllOfCurrent_WithValidData_ShouldReturnListOfSubjectDtos(string role)
        {
            this.teacherRepository.Entities.Clear();
            this.studentRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(a => a.GetEmail())
                .Returns("email");
            this.currentUserMock
                .Setup(a => a.GetRoles())
                .Returns(new string[] { role });

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var student = new Student
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.studentRepository.Entities.Add(student);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            subject.Students.Add(student);

            var result = await this.subjectService.GetAllOfCurrentAsync();

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0.Count, Is.EqualTo(1));
            });
        }

        [TestCase("Teacher")]
        [TestCase("Student")]
        public async Task GetAllOfCurrent_WithInvalidData_ShouldReturnUserNotFound(string role)
        {
            this.teacherRepository.Entities.Clear();
            this.studentRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            this.currentUserMock.Reset();
            this.currentUserMock
                .Setup(a => a.GetEmail())
                .Returns("email");
            this.currentUserMock
                .Setup(a => a.GetRoles())
                .Returns(new string[] { role });

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var student = new Student
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            subject.Students.Add(student);

            var result = await this.subjectService.GetAllOfCurrentAsync();

            Assert.That(result.IsT1);
        }
        #endregion GetAllOFCurrent

        #region GetSubjectOfTeacher
        [Test]
        public async Task GetSubjectOfTeacherById_WithValidData_ShouldReturnSubjectInfoDto()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.subjectService.GetSubjectOfTeacherByIdAsync(subject.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0.CustomId, Is.EqualTo("customid"));
                Assert.That(result.AsT0.Title, Is.EqualTo("title"));
                Assert.That(result.AsT0.TeacherEmail, Is.EqualTo("email"));
            });
        }

        [Test]
        public async Task GetSubjectOfTeacherById_WithNonexistentSubject_ShouldReturnNotFound()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.subjectService.GetSubjectOfTeacherByIdAsync(subject.Id+1);

            Assert.That(result.IsT2);
        }

        [Test]
        public async Task GetSubjectOfTeacherById_WithNonexistentTeacher_ShouldReturnUserNotFound()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.subjectService.GetSubjectOfTeacherByIdAsync(subject.Id);

            Assert.That(result.IsT1);
        }
        #endregion GetSubjectOfTeacher

        #region GetSubjectOfStudent
        [Test]
        public async Task GetSubjectOfStudentById_WithValidData_ShouldReturnSubjectInfoDto()
        {
            this.studentRepository.Entities.Clear();
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var student = new Student
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.studentRepository.Entities.Add(student);

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            subject.Students.Add(student);

            this.subjectRepository.Entities.Add(subject);

            var result = await this.subjectService.GetSubjectOfStudentByIdAsync(subject.Id);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsT0);
                Assert.That(result.AsT0.CustomId, Is.EqualTo("customid"));
                Assert.That(result.AsT0.Title, Is.EqualTo("title"));
                Assert.That(result.AsT0.TeacherEmail, Is.EqualTo("email"));
            });
        }

        [Test]
        public async Task GetSubjectOfStudentById_WithNonexistentSubject_ShouldReturnNotFound()
        {
            this.studentRepository.Entities.Clear();
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var student = new Student
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.studentRepository.Entities.Add(student);

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            subject.Students.Add(student);

            this.subjectRepository.Entities.Add(subject);

            var result = await this.subjectService.GetSubjectOfStudentByIdAsync(subject.Id+1);

            Assert.That(result.IsT2);
        }

        [Test]
        public async Task GetSubjectOfStudentById_WithNonexistentStudent_ShouldReturnUserNotFound()
        {
            this.studentRepository.Entities.Clear();
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var student = new Student
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.studentRepository.Entities.Add(student);

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            subject.Students.Add(student);

            this.subjectRepository.Entities.Add(subject);

            var result = await this.subjectService.GetSubjectOfStudentByIdAsync(subject.Id);

            Assert.That(result.IsT1);
        }
        #endregion GetSubjectOfStudent

        #region UpdateSubjectInfo
        [Test]
        public async Task UpdateSubjectInfo_WithValidData_ShouldReturnSuccess()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var updateDto = new UpdateSubjectDto
            {
                CustomId = "newcustomid",
                Title = "newtitle"
            };

            var result = await this.subjectService.UpdateSubjectInfoAsync(subject.Id, updateDto);

            Assert.That(result.IsT0);
        }

        [Test]
        public async Task UpdateSubjectInfo_WithValidData_ShouldUpdateSubject()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var updateDto = new UpdateSubjectDto
            {
                CustomId = "newcustomid",
                Title = "newtitle"
            };

            await this.subjectService.UpdateSubjectInfoAsync(subject.Id, updateDto);

            var updatedSubject = this.subjectRepository.Entities.FirstOrDefault(x => x.Id == subject.Id);

            Assert.That(updatedSubject, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(updatedSubject.CustomId, Is.EqualTo("newcustomid"));
                Assert.That(updatedSubject.Title, Is.EqualTo("newtitle"));
            });
        }

        [Test]
        public async Task UpdateSubjectInfo_WithNonexistentSubject_ShouldReturnNotFound()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var updateDto = new UpdateSubjectDto
            {
                CustomId = "newcustomid",
                Title = "newtitle"
            };

            var result = await this.subjectService.UpdateSubjectInfoAsync(subject.Id+1, updateDto);

            Assert.That(result.IsT3);
        }

        [Test]
        public async Task UpdateSubjectInfo_WithNonexistentTeacher_ShouldReturnUserNotFound()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var updateDto = new UpdateSubjectDto
            {
                CustomId = "newcustomid",
                Title = "newtitle"
            };

            var result = await this.subjectService.UpdateSubjectInfoAsync(subject.Id, updateDto);

            Assert.That(result.IsT2);
        }
        #endregion UpdateSubjectInfo

        #region DeleteById
        [Test]
        public async Task DeleteById_WithValidData_ShouldReturnSuccess()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.subjectService.DeleteByIdAsync(subject.Id);

            Assert.That(result.IsT0);
        }

        [Test]
        public async Task DeleteById_WithValidData_ShouldDeleteSubject()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            await this.subjectService.DeleteByIdAsync(subject.Id);

            Assert.That(this.subjectRepository.Entities, Is.Empty);
        }

        [Test]
        public async Task DeleteById_WithNonexistentSubject_ShouldReturnNotFound()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "email",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.subjectService.DeleteByIdAsync(subject.Id + 1);

            Assert.That(result.IsT2);
        }

        [Test]
        public async Task DeleteById_WithNonexistentTeacher_ShouldReturnUserNotFound()
        {
            this.teacherRepository.Entities.Clear();
            this.subjectRepository.Entities.Clear();

            var teacher = new Teacher
            {
                Email = "otheremail",
                PasswordHash = "$2a$11$iH1gMH58861GM7rnUaAJY.horJrbmQOfskOYX/2n9Npuk0Pu.bc2i",
                Name = "name",
                Surname = "surname",
            };

            this.teacherRepository.Entities.Add(teacher);

            var subject = new Subject
            {
                CustomId = "customid",
                Title = "title",
                TeacherId = teacher.Id,
            };

            this.subjectRepository.Entities.Add(subject);

            var result = await this.subjectService.DeleteByIdAsync(subject.Id);

            Assert.That(result.IsT1);
        }
        #endregion DeleteById
    }
}

using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Tests.Fakes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
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

            this.authenticationService =
                new AuthenticationService(this.studentRepository, this.teacherRepository);
        }

        [Test]
        public async Task RegisterStudent_WithValidData_ShouldReturnSuccess()
        {
            this.studentRepository.Entities.Clear();
            var student = new StudentDto
            {
                AuthInfo = new UserAuthDto
                {
                    Email = "email",
                    Password = "password",
                },

                StudentInfo = new StudentInfoDto
                {
                    Name = "name",
                    Surname = "surname",
                    CustomId = "customId"
                },
            };

            var result = await this.authenticationService.RegisterStudent(student);
            Assert.That(result.IsSuccess(), Is.True);
        }

        [Test]
        public async Task RegisterStudent_WithValidData_ShouldAddStudent()
        {
            this.studentRepository.Entities.Clear();
            var student = new StudentDto
            {
                AuthInfo = new UserAuthDto
                {
                    Email = "email",
                    Password = "password",
                },

                StudentInfo = new StudentInfoDto
                {
                    Name = "name",
                    Surname = "surname",
                    CustomId = "customId"
                },
            };

            await this.authenticationService.RegisterStudent(student);
            var dbStudent = this.studentRepository.GetAll().FirstOrDefault();

            Assert.That(dbStudent, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(dbStudent.Email, Is.EqualTo(student.AuthInfo.Email));
                Assert.That(dbStudent.Name, Is.EqualTo(student.StudentInfo.Name));
                Assert.That(dbStudent.Surname, Is.EqualTo(student.StudentInfo.Surname));
                Assert.That(dbStudent.CustomId, Is.EqualTo(student.StudentInfo.CustomId));
            });
        }

        [Test]
        public async Task RegisterStudent_WithExistingEmail_ShouldReturnFailure()
        {
            this.studentRepository.Entities.Clear();
            var student = new StudentDto
            {
                AuthInfo = new UserAuthDto
                {
                    Email = "email",
                    Password = "password",
                },

                StudentInfo = new StudentInfoDto
                {
                    Name = "name",
                    Surname = "surname",
                    CustomId = "customId"
                },
            };

            await this.authenticationService.RegisterStudent(student);
            var result = await this.authenticationService.RegisterStudent(student);
            Assert.That(result.IsFailure(), Is.True);
        }

        [Test]
        public async Task RegisterStudent_WithExistingEmail_ShouldNotAddStudent()
        {
            this.studentRepository.Entities.Clear();
            var student = new StudentDto
            {
                AuthInfo = new UserAuthDto
                {
                    Email = "email",
                    Password = "password",
                },

                StudentInfo = new StudentInfoDto
                {
                    Name = "name",
                    Surname = "surname",
                    CustomId = "customId"
                },
            };

            await this.authenticationService.RegisterStudent(student);
            await this.authenticationService.RegisterStudent(student);

            var studentsCount = this.studentRepository.GetAll().Count();
            Assert.That(studentsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task LoginStudent_WithValidData_ShouldReturnSuccess()
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

            var result = await this.authenticationService.LoginStudent(student, "Secret key token");
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess(out var token), Is.True);
                Assert.That(token, Is.Not.Null);
            });
        }

        [Test]
        public async Task LoginStudent_WithInvalidData_ShouldReturnFailure()
        {
            this.studentRepository.Entities.Clear();
            var student = new UserAuthDto
            {
                Email = "email",
                Password = "password",
            };

            var result = await this.authenticationService.LoginStudent(student, "Secret key token");
            Assert.That(result.IsFailure(), Is.True);
        }

        [Test]
        public async Task RegisterTeacher_WithValidData_ShouldReturnSuccess()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new TeacherDto
            {
                AuthInfo = new UserAuthDto
                {
                    Email = "email",
                    Password = "password",
                },

                TeacherInfo = new TeacherInfoDto
                {
                    Name = "name",
                    Surname = "surname",
                },
            };

            var result = await this.authenticationService.RegisterTeacher(teacher);
            Assert.That(result.IsSuccess(), Is.True);
        }

        [Test]
        public async Task RegisterTeacher_WithValidData_ShouldAddTeacher()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new TeacherDto
            {
                AuthInfo = new UserAuthDto
                {
                    Email = "email",
                    Password = "password",
                },

                TeacherInfo = new TeacherInfoDto
                {
                    Name = "name",
                    Surname = "surname",
                },
            };

            await this.authenticationService.RegisterTeacher(teacher);
            var dbTeacher = this.teacherRepository.GetAll().FirstOrDefault();
            Assert.That(dbTeacher, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(dbTeacher.Email, Is.EqualTo(teacher.AuthInfo.Email));
                Assert.That(dbTeacher.Name, Is.EqualTo(teacher.TeacherInfo.Name));
                Assert.That(dbTeacher.Surname, Is.EqualTo(teacher.TeacherInfo.Surname));
            });
        }

        [Test]
        public async Task RegisterTeacher_WithExistingEmail_ShouldReturnFailure()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new TeacherDto
            {
                AuthInfo = new UserAuthDto
                {
                    Email = "email",
                    Password = "password",
                },

                TeacherInfo = new TeacherInfoDto
                {
                    Name = "name",
                    Surname = "surname",
                },
            };

            await this.authenticationService.RegisterTeacher(teacher);
            var result = await this.authenticationService.RegisterTeacher(teacher);
            Assert.That(result.IsFailure());
        }

        [Test]
        public async Task RegisterTeacher_WithExistingEmail_ShouldNotAddTeacher()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new TeacherDto
            {
                AuthInfo = new UserAuthDto
                {
                    Email = "email",
                    Password = "password",
                },

                TeacherInfo = new TeacherInfoDto
                {
                    Name = "name",
                    Surname = "surname",
                },
            };

            await this.authenticationService.RegisterTeacher(teacher);
            await this.authenticationService.RegisterTeacher(teacher);

            var teachersCount = this.teacherRepository.GetAll().Count();
            Assert.That(teachersCount, Is.EqualTo(1));
        }

        [Test]
        public async Task LoginTeacher_WithValidData_ShouldReturnSuccess()
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

            var result = await this.authenticationService.LoginTeacher(teacher, "Secret key token");
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess(out var token), Is.True);
                Assert.That(token, Is.Not.Null);
            });
        }

        [Test]
        public async Task LoginTeacher_WithInvalidData_ShouldReturnFailure()
        {
            this.teacherRepository.Entities.Clear();
            var teacher = new UserAuthDto
            {
                Email = "email",
                Password = "password",
            };

            var result = await this.authenticationService.LoginTeacher(teacher, "Secret key token");
            Assert.That(result.IsFailure(), Is.True);
        }
    }
}

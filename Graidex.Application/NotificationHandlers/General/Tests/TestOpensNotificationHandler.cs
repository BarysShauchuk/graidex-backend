using Graidex.Application.Notifications.Tests.Opens;
using Graidex.Application.Notifications.Tests.Opens.Student;
using Graidex.Application.Notifications.Tests.Opens.Teacher;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.NotificationHandlers.General.Tests
{
    public class TestOpensNotificationHandler : INotificationHandler<TestOpensNotification>
    {
        private readonly IMediator mediator;
        private readonly ISubjectRepository subjectRepository;
        private readonly ITestRepository testRepository;
        private readonly ITeacherRepository teacherRepository;

        public TestOpensNotificationHandler(
            IMediator mediator,
            IServiceProvider serviceProvider)
        {
            this.mediator = mediator;

            var scopeProvider = serviceProvider.CreateScope().ServiceProvider;
            this.subjectRepository = scopeProvider.GetRequiredService<ISubjectRepository>();
            this.testRepository = scopeProvider.GetRequiredService<ITestRepository>();
            this.teacherRepository = scopeProvider.GetRequiredService<ITeacherRepository>();
        }

        public async Task Handle(TestOpensNotification notification, CancellationToken cancellationToken)
        {
            var test = await this.testRepository.GetById(notification.TestId);
            if (test is null)
            {
                return;
            }

            var studentEmails = test.AllowedStudents
                .Select(student => student.Email)
                .ToArray();

            await this.NotifyTeacher(test, studentEmails.Length);
            await this.NotifyStudents(test, studentEmails);
        }

        private async Task NotifyTeacher(Test test, int studentCount)
        {
            var teacherId = (await this.subjectRepository.GetById(test.SubjectId))?.TeacherId;
            if (teacherId is null)
            {
                return;
            }

            var teacherEmail = (await this.teacherRepository.GetById(teacherId.Value))?.Email;
            if (teacherEmail is null)
            {
                return;
            }

            var teacherNotification = new TestOpensTeacherNotification
            {
                TeacherEmail = teacherEmail,
                Data = new()
                {
                    TestId = test.Id,
                    TestTitle = test.Title,
                    IsVisible = test.IsVisible,
                    StudentCount = studentCount,
                }
            };

            await this.mediator.Publish(teacherNotification);
        }

        private async Task NotifyStudents(Test test, string[] emails)
        {
            if (!test.IsVisible)
            {
                return;
            }

            var studentNotification = new TestOpensStudentNotification
            {
                StudentEmails = emails,
                Data = new()
                {
                    TestId = test.Id,
                    TestTitle = test.Title,
                    EndDateTime = test.EndDateTime,
                }
            };

            await this.mediator.Publish(studentNotification);
        }
    }
}

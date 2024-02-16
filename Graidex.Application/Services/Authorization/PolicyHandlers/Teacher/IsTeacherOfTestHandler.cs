using Graidex.Application.Interfaces;
using Graidex.Application.Services.Authorization.Requirements.Teacher;
using Graidex.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Authorization.PolicyHandlers.Teacher
{
    public class IsTeacherOfTestHandler : AuthorizationHandler<IsTeacherOfTestRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly ITestRepository testRepository;

        public IsTeacherOfTestHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            ITestRepository testRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.testRepository = testRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsTeacherOfTestRequirement requirement)
        {
            if (!context.User.IsInRole("Teacher"))
            {
                context.Fail();
                return;
            }

            string teacherEmail = currentUser.GetEmail();
            var teacher = await teacherRepository.GetByEmail(teacherEmail);
            if (teacher is null)
            {
                context.Fail();
                return;
            }

            int testId = Convert.ToInt32(routeData.RouteValues["testId"]);
            if (testId == 0)
            {
                context.Fail();
                return;
            }

            var test = await testRepository.GetById(testId);
            if (test is null)
            {
                context.Fail();
                return;
            }

            var subject = await subjectRepository.GetById(test.SubjectId);
            if (subject is null)
            {
                context.Fail();
                return;
            }

            if (subject.TeacherId == teacher.Id)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}

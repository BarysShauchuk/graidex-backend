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
    public class IsTeacherOfTestResultHandler : AuthorizationHandler<IsTeacherOfTestResultRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly ITestRepository testRepository;
        private readonly ITestResultRepository testResultRepository;

        public IsTeacherOfTestResultHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            ITestRepository testRepository,
            ITestResultRepository testResultRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.testRepository = testRepository;
            this.testResultRepository = testResultRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsTeacherOfTestResultRequirement requirement)
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

            int testResultId = Convert.ToInt32(routeData.RouteValues["testResultId"]);
            if (testResultId == 0)
            {
                context.Fail();
                return;
            }

            var testResult = await testResultRepository.GetById(testResultId);
            if (testResult is null)
            {
                context.Fail();
                return;
            }

            var test = await testRepository.GetById(testResult.TestId);
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

            if (subject.TeacherId != teacher.Id)
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
    }
}

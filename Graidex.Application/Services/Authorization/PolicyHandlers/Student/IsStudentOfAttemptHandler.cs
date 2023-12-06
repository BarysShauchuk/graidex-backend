using Graidex.Application.Interfaces;
using Graidex.Application.Services.Authorization.Requirements.Student;
using Graidex.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Authorization.PolicyHandlers.Student
{
    public class IsStudentOfAttemptHandler : AuthorizationHandler<IsStudentOfAttemptRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly IStudentRepository studentRepository;
        private readonly ITestResultRepository testResultRepository;

        public IsStudentOfAttemptHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            IStudentRepository studentRepository,
            ITestResultRepository testResultRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.studentRepository = studentRepository;
            this.testResultRepository = testResultRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsStudentOfAttemptRequirement requirement)
        {
            if (!context.User.IsInRole("Student"))
            {
                context.Fail();
                return;
            }

            string studentEmail = currentUser.GetEmail();
            var student = await studentRepository.GetByEmail(studentEmail);
            if (student is null)
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

            if (testResult.StudentId == student.Id)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}

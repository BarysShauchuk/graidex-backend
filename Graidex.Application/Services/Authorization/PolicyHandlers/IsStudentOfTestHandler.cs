using Graidex.Application.Interfaces;
using Graidex.Application.Services.Authorization.Requirements;
using Graidex.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Authorization.PolicyHandlers
{
    public class IsStudentOfTestHandler : AuthorizationHandler<IsStudentOfTestRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly IStudentRepository studentRepository;
        private readonly ITestRepository testRepository;

        public IsStudentOfTestHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            IStudentRepository studentRepository,
            ITestRepository testRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.studentRepository = studentRepository;
            this.testRepository = testRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsStudentOfTestRequirement requirement)
        {
            if (!context.User.IsInRole("Student"))
            {
                context.Fail();
                return;
            }

            string studentEmail = this.currentUser.GetEmail();
            var student = await this.studentRepository.GetByEmail(studentEmail);
            if (student is null)
            {
                context.Fail();
                return;
            }

            int testId = Convert.ToInt32(this.routeData.RouteValues["testId"]);
            if (testId == 0)
            {
                context.Fail();
                return;
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                context.Fail();
                return;
            }

            if (!test.AllowedStudents.Contains(student))
            {
                context.Fail();
                return;
            }

            if (test.IsVisible || (test.StartDateTime > DateTime.Now && test.EndDateTime < DateTime.Now))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}

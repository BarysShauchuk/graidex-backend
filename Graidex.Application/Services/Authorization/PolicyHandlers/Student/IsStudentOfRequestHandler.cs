using Graidex.Application.Interfaces;
using Graidex.Application.Services.Authorization.Requirements.Student;
using Graidex.Application.Services.Users;
using Graidex.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Authorization.PolicyHandlers.Student
{
    public class IsStudentOfRequestHandler : AuthorizationHandler<IsStudentOfRequestRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly IStudentRepository studentRepository;
        private readonly ISubjectRequestRepository subjectRequestRepository;

        public IsStudentOfRequestHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            IStudentRepository studentRepository,
            ISubjectRequestRepository subjectRequestRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.studentRepository = studentRepository;
            this.subjectRequestRepository = subjectRequestRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsStudentOfRequestRequirement requirement)
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

            int subjectRequestId = Convert.ToInt32(routeData.RouteValues["subjectRequestId"]);
            if (subjectRequestId == 0)
            {
                context.Fail();
                return;
            }

            var subjectRequest = await subjectRequestRepository.GetById(subjectRequestId);
            if (subjectRequest is null)
            {
                context.Fail();
                return;
            }

            if (subjectRequest.StudentId == student.Id)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}

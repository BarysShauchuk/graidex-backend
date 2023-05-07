using Graidex.Application.Interfaces;
using Graidex.Application.Services.Authorization.Requirements;
using Graidex.Application.Services.Users;
using Graidex.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Authorization.PolicyHandlers
{
    public class IsStudentOfSubjectHandler : AuthorizationHandler<IsStudentOfSubjectRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly IStudentRepository studentRepository;
        private readonly ISubjectRepository subjectRepository;

        public IsStudentOfSubjectHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            IStudentRepository studentRepository,
            ISubjectRepository subjectRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.studentRepository = studentRepository;
            this.subjectRepository = subjectRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsStudentOfSubjectRequirement requirement)
        {
            string studentEmail;
            try
            {
                studentEmail = this.currentUser.GetEmail();
            }
            catch (HttpRequestException)
            {
                context.Fail();
                return;
            }

            var student = await this.studentRepository.GetByEmail(studentEmail);
            if (student is null)
            {
                context.Fail();
                return;
            }

            int subjectId = Convert.ToInt32(this.routeData.RouteValues["subjectId"]);
            if (subjectId == 0)
            {
                context.Fail();
                return;
            }

            var subject = await this.subjectRepository.GetById(subjectId);
            if (subject is null)
            {
                context.Fail();
                return;
            }

            if (subject.Students.Any(x => x.Id == student.Id))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}

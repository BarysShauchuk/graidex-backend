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
    public class IsTeacherOfRequestHandler : AuthorizationHandler<IsTeacherOfRequestRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly ISubjectRequestRepository subjectRequestRepository;

        public IsTeacherOfRequestHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            ISubjectRequestRepository subjectRequestRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.subjectRequestRepository = subjectRequestRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsTeacherOfRequestRequirement requirement)
        {
            if (!context.User.IsInRole("Teacher"))
            {
                context.Fail();
                return;
            }

            string teacherEmail = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(teacherEmail);
            if (teacher is null)
            {
                context.Fail();
                return;
            }

            int subjectRequestId = Convert.ToInt32(this.routeData.RouteValues["subjectRequestId"]);
            if (subjectRequestId == 0)
            {
                context.Fail();
                return;
            }

            var subjectRequest = await this.subjectRequestRepository.GetById(subjectRequestId);
            if (subjectRequest is null)
            {
                context.Fail();
                return;
            }

            var subject = await subjectRepository.GetById(subjectRequest.SubjectId);
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

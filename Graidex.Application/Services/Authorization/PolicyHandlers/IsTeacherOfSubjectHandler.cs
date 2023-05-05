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
    public class IsTeacherOfSubjectHandler : AuthorizationHandler<IsTeacherOfSubjectRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;

        public IsTeacherOfSubjectHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsTeacherOfSubjectRequirement requirement)
        {
            string teacherEmail = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(teacherEmail);
            if (teacher is null)
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

            if (subject.TeacherId == teacher.Id)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}

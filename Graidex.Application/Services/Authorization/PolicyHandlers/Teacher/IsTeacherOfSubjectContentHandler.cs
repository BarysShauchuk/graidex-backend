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
    public class IsTeacherOfSubjectContentHandler : AuthorizationHandler<IsTeacherOfSubjectContentRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;

        public IsTeacherOfSubjectContentHandler(
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
            IsTeacherOfSubjectContentRequirement requirement)
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

            int contentId = Convert.ToInt32(routeData.RouteValues["contentId"]);
            if (contentId == 0)
            {
                context.Fail();
                return;
            }

            var content = await subjectRepository.GetContentItemById(contentId);
            if (content is null)
            {
                context.Fail();
                return;
            }

            var subject = await subjectRepository.GetById(content.SubjectId);
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

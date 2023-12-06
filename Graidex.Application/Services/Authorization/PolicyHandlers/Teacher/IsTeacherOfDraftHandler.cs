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
    public class IsTeacherOfDraftHandler : AuthorizationHandler<IsTeacherOfDraftRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly ITestDraftRepository testDraftRepository;

        public IsTeacherOfDraftHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            ITestDraftRepository testDraftRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.testDraftRepository = testDraftRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsTeacherOfDraftRequirement requirement)
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

            int draftId = Convert.ToInt32(routeData.RouteValues["draftId"]);
            if (draftId == 0)
            {
                context.Fail();
                return;
            }

            var draft = await testDraftRepository.GetById(draftId);
            if (draft is null)
            {
                context.Fail();
                return;
            }

            var subject = await subjectRepository.GetById(draft.SubjectId);
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

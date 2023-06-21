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
    public class IsTeacherOfStudentHandler : AuthorizationHandler<IsTeacherOfStudentRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;

        public IsTeacherOfStudentHandler(
            ICurrentUserService currentUser,
            IRouteDataService routeData,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository)
        {
            this.currentUser = currentUser;
            this.routeData = routeData;
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsTeacherOfStudentRequirement requirement)
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

            string? studentEmail = Convert.ToString(this.routeData.RouteValues["studentEmail"]);
            if (studentEmail is null)
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

            bool haveCommonSubjects = this.subjectRepository
                .GetAll()
                .Where(x => x.TeacherId == teacher.Id)
                .Any(x => x.Students.Any(x => x.Id == student.Id));

            if (haveCommonSubjects)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}

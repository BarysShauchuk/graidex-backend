﻿using Graidex.Application.Interfaces;
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
    public class IsStudentOfVisibleTestHandler : AuthorizationHandler<IsStudentOfVisibleTestRequirement>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IRouteDataService routeData;
        private readonly IStudentRepository studentRepository;
        private readonly ITestRepository testRepository;

        public IsStudentOfVisibleTestHandler(
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
            IsStudentOfVisibleTestRequirement requirement)
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

            int testId = Convert.ToInt32(routeData.RouteValues["testId"]);
            if (testId == 0)
            {
                context.Fail();
                return;
            }

            var test = await testRepository.GetById(testId);
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

            if (test.IsVisible || DateTime.UtcNow > test.StartDateTime && DateTime.UtcNow < test.EndDateTime)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}
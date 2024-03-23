using Graidex.Domain.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graidex.Domain.Models.Tests.Test;

namespace Graidex.Application.Factories.Tests
{
    public class TestBaseFactory : ITestBaseFactory
    {
        public TestDraft CreateTestDraft(Test test)
        {
            var testDraft = new TestDraft
            {
                Title = test.Title,
                Description = test.Description,
                GradeToPass = test.GradeToPass,
                MaxPoints = test.MaxPoints,
                SubjectId = test.SubjectId,
                LastUpdate = DateTime.UtcNow,
            };

            return testDraft;
        }

        public TestDraft DuplicateTestDraft(TestDraft testDraft)
        {
            var clone = testDraft.CreateExactClone();
            clone.Id = 0;
            clone.LastUpdate = DateTime.UtcNow;

            return clone;
        }

        public Test CreateTest(TestDraft testDraft, TestDraftToTestParameters parameters)
        {
            var test = new Test
            {
                Title = testDraft.Title,
                Description = testDraft.Description,
                GradeToPass = testDraft.GradeToPass,
                MaxPoints = testDraft.MaxPoints,
                SubjectId = testDraft.SubjectId,

                StartDateTime = parameters.StartDateTime,
                EndDateTime = parameters.EndDateTime,
                TimeLimit = parameters.TimeLimit,
                AutoCheckAfterSubmission = parameters.AutoCheckAfterSubmission,
                ShowToStudent = parameters.ReviewResult,
                ShuffleQuestions = parameters.ShuffleQuestions,
            };

            return test;
        }
    }
}

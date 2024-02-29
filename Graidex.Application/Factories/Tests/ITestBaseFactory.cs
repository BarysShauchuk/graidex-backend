using Graidex.Domain.Models.Tests;

namespace Graidex.Application.Factories.Tests
{
    public interface ITestBaseFactory
    {
        public Test CreateTest(TestDraft testDraft, TestDraftToTestParameters parameters);
        public TestDraft CreateTestDraft(Test test);
        public TestDraft DuplicateTestDraft(TestDraft testDraft);
    }
}
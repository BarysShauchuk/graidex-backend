using Graidex.Application.OneOfCustomTypes;
using OneOf.Types;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;

namespace Graidex.Application.Services.Tests
{
    public interface ITestResultService
    {
        public Task<OneOf<Success, UserNotFound, NotFound, OutOfAttempts>> StartTestAttemptAsync(int testId);
        public Task<OneOf<Success, NotFound, AttemptFinished>> UpdateTestAttemptByIdAsync(int testResultId);
        public Task<OneOf<Success, NotFound>> SubmitTestAttemptByIdAsync(int testResultId);
    }
}

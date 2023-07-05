using Graidex.Application.DTOs.Test.TestAttempt;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Tests
{
    public interface ITestService
    {
        public Task<OneOf<Success, Error>> StartTestAttemptAsync(InitialTestAttemptDto testAttempt);
        public Task<OneOf<Success, Error>> SubmitTestAttemptAsync(FinalTestAttemptDto testAttempt);
    }
}

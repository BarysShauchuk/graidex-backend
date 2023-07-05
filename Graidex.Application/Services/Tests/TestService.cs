using AutoMapper;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.Interfaces;
using Graidex.Application.Interfaces.TestCheckingQueue;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Tests
{
    public class TestService : ITestService
    {
        private readonly ITestRepository testRepository;
        private readonly ITestResultRepository testResultRepository;
        private readonly IMapper mapper;
        private readonly ITestCheckingInQueue testCheckingQueue;

        public TestService(
            ITestRepository testRepository,
            ITestResultRepository testResultRepository,
            IMapper mapper,
            ITestCheckingInQueue testCheckingQueue)
        {
            this.testRepository = testRepository;
            this.testResultRepository = testResultRepository;
            this.mapper = mapper;
            this.testCheckingQueue = testCheckingQueue;
        }

        public async Task<OneOf<Success, Error>> StartTestAttemptAsync(InitialTestAttemptDto testAttempt)
        {
            throw new NotImplementedException();
        }

        public async Task<OneOf<Success, Error>> SubmitTestAttemptAsync(FinalTestAttemptDto testAttempt)
        {
            // Validation, checks, logic, etc.

            //var testResult = mapper.Map<TestResult>(testAttempt);
            //await this.testResultRepository.Add(testResult);
            //await this.testCheckingQueue.AddAsync(testResult.Id);
            //return new Success();

            throw new NotImplementedException();
        }
    }
}

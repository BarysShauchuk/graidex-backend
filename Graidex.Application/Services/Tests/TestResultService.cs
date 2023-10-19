using AutoMapper;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.Interfaces;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Graidex.Application.Services.Tests
{
    public class TestResultService : ITestResultService
    {
        private readonly ICurrentUserService currentUser;
        private readonly IStudentRepository studentRepository;
        private readonly ITestRepository testRepository;
        private readonly ITestResultRepository testResultRepository;
        private readonly IMapper mapper;

        public TestResultService(
            ICurrentUserService currentUser,
            IStudentRepository studentRepository,
            ITestRepository testRepository,
            ITestResultRepository testResultRepository,
            IMapper mapper)
        {
            this.currentUser = currentUser;
            this.studentRepository = studentRepository;
            this.testRepository = testRepository;
            this.testResultRepository = testResultRepository;
            this.mapper = mapper;
        }

        public async Task<OneOf<Success, UserNotFound, NotFound, OutOfAttempts>> StartTestAttemptAsync(int testId)
        {
            string email = this.currentUser.GetEmail();
            var student = await studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            bool attemptAlreadyStarted = this.testResultRepository
                .GetAll()
                .Any(x => x.StudentId == student.Id 
                && x.TestId == test.Id);

            if (attemptAlreadyStarted)
            {
                return new OutOfAttempts("No more attempts available");
            }

            var testResult = new TestResult
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now + test.TimeLimit,
                TestId = testId,
                StudentId = student.Id
            };

            await this.testResultRepository.Add(testResult);

            return new Success();
        }

        public async Task<OneOf<Success, NotFound, AttemptFinished>> UpdateTestAttemptByIdAsync(int testResultId)
        {
            var testAttempt = await this.testResultRepository.GetById(testResultId);
            if (testAttempt is null)
            {
                return new NotFound();
            }

            var test = await this.testRepository.GetById(testAttempt.TestId);
            if (test is null)
            {
                return new NotFound();
            }

            if (DateTime.Now > test.EndDateTime 
                || DateTime.Now > testAttempt.StartTime + test.TimeLimit 
                || DateTime.Now > testAttempt.EndTime)
            {
                return new AttemptFinished("This test attempt is already finished");
            }

            // TODO: Add answers update logic

            await this.testResultRepository.Update(testAttempt);

            return new Success();
        }

        public async Task<OneOf<Success, NotFound>> SubmitTestAttemptByIdAsync(int testResultId)
        {
            var updateResult = await this.UpdateTestAttemptByIdAsync(testResultId);
            if (updateResult.IsT1)
            {
                return new NotFound();
            }

            if (updateResult.IsT2)
            {
                return new Success();
            }

            var testAttempt = await this.testResultRepository.GetById(testResultId);
            if (testAttempt is null)
            {
                return new NotFound();
            }

            testAttempt.EndTime = DateTime.Now;

            await this.testResultRepository.Update(testAttempt);

            return new Success();
        }
    }
}

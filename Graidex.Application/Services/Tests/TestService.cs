using AutoMapper;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.DTOs.Test.TestDraft;
using Graidex.Application.Interfaces;
using Graidex.Application.Interfaces.TestCheckingQueue;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Tests;
using Graidex.Domain.Models.Users;
using Graidex.Domain.Models.Tests.Questions;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using FluentValidation;

namespace Graidex.Application.Services.Tests
{
    public class TestService : ITestService
    {
        private readonly ICurrentUserService currentUser;
        private readonly ITeacherRepository teacherRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ITestRepository testRepository;
        private readonly ITestDraftRepository testDraftRepository;
        private readonly ITestBaseQuestionsRepository testBaseQuestionsRepository;
        private readonly ITestResultRepository testResultRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;
        private readonly ITestCheckingInQueue testCheckingQueue;
        private readonly IValidator<List<TestBaseQuestionDto>> testBaseQuestionsListValidator;

        public TestService(
            ICurrentUserService currentUser,
            ITeacherRepository teacherRepository,
            IStudentRepository studentRepository,
            ITestRepository testRepository,
            ITestDraftRepository testDraftRepository,
            ITestBaseQuestionsRepository testBaseQuestionsRepository,
            ITestResultRepository testResultRepository,
            ISubjectRepository subjectRepository,
            IMapper mapper,
            ITestCheckingInQueue testCheckingQueue,
            IValidator<List<TestBaseQuestionDto>> testBaseQuestionsListValidator)
        {   
            this.currentUser = currentUser;
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
            this.testRepository = testRepository;
            this.testDraftRepository = testDraftRepository;
            this.testBaseQuestionsRepository = testBaseQuestionsRepository;
            this.testResultRepository = testResultRepository;
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
            this.testCheckingQueue = testCheckingQueue;
            this.testBaseQuestionsListValidator = testBaseQuestionsListValidator;
        }

        public async Task<OneOf<List<TestBaseQuestionDto>, NotFound>> GetTestQuestionsAsync(int testId)
        {
            var result = await this.GetTestBaseQuestionsAsync(testId, this.testRepository);
            return result;
        }

        public async Task<OneOf<GetTestDraftDto, ValidationFailed, UserNotFound>> CreateTestDraftForSubjectAsync(int subjectId, CreateTestDraftDto createTestDraftDto)
        {   
            // TODO: Add validation

            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var testDraft = this.mapper.Map<TestDraft>(createTestDraftDto);

            testDraft.SubjectId = subjectId;
            testDraft.LastUpdate = DateTime.Now;

            await this.testDraftRepository.Add(testDraft);

            return this.mapper.Map<GetTestDraftDto>(testDraft);
        }

        public async Task<OneOf<GetTestDraftDto, UserNotFound, NotFound>> CreateTestDraftFromTestAsync(int testId)
        {
            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            var createTestDraftDto = this.mapper.Map<CreateTestDraftDto>(test);

            var testDraft = this.mapper.Map<TestDraft>(createTestDraftDto);
            testDraft.SubjectId = test.SubjectId;
            testDraft.LastUpdate = DateTime.Now;

            await this.testDraftRepository.Add(testDraft);

            return this.mapper.Map<GetTestDraftDto> (testDraft);    
        }

        public async Task<OneOf<GetTestDraftDto, UserNotFound, NotFound>> DuplicateTestDraftAsync(int draftId)
        {
            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            var duplicateDraftDto = this.mapper.Map<DuplicateDraftDto>(testDraft);

            var duplicateDraft = this.mapper.Map<TestDraft>(duplicateDraftDto);

            await this.testDraftRepository.Add(duplicateDraft);

            return this.mapper.Map<GetTestDraftDto>(duplicateDraft);
        }

        public async Task<OneOf<GetTestDraftDto, UserNotFound, NotFound>> GetTestDraftByIdAsync(int draftId)
        {
            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            return this.mapper.Map<GetTestDraftDto>(testDraft);
        }

        public async Task<OneOf<Success,  ValidationFailed, UserNotFound, NotFound>> UpdateTestDraftByIdASync(int draftId, UpdateTestDraftDto updateTestDraftDto)
        {
            // TODO: Add validation

            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            this.mapper.Map(updateTestDraftDto, testDraft);

            testDraft.LastUpdate = DateTime.Now;
            await testDraftRepository.Update(testDraft);

            return new Success();
        }

        public async Task<OneOf<Success, UserNotFound, NotFound>> DeleteTestDraftByIdAsync(int draftId)
        {
            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            await this.testDraftRepository.Delete(testDraft);

            return new Success();
        }

        public async Task<OneOf<GetTestDto, ValidationFailed, UserNotFound, NotFound>> CreateTestForDraftAsync(int draftId, CreateTestDto createTestDto)
        {   
            // TODO: Add validation

            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            var testDraftDto = this.mapper.Map<DraftToTestDto>(testDraft);

            var test = this.mapper.Map<Test>(testDraftDto);
            mapper.Map(createTestDto, test);

            await this.testRepository.Add(test);

            return this.mapper.Map<GetTestDto>(test);
        }

        public async Task<OneOf<GetTestDto, UserNotFound, NotFound>> GetTestByIdAsync(int testId)
        {
            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }
            return this.mapper.Map<GetTestDto>(test);
        }

        public async Task<OneOf<Success, ValidationFailed, UserNotFound, NotFound, TestImmutable>> UpdateTestByIdAsync(int testId, UpdateTestDto updateTestDto)
        {
            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            if (DateTime.Now >= test.StartDateTime && DateTime.Now < test.EndDateTime)
            {   
                if (updateTestDto.StartDateTime != test.StartDateTime || updateTestDto.TimeLimit < test.TimeLimit)
                {
                    return new TestImmutable("Test has already started");
                }
            }

            if (DateTime.Now >= test.EndDateTime)
            {
                return new TestImmutable("Test has already ended");
            }

            this.mapper.Map(updateTestDto, test);

            await testRepository.Update(test);

            return new Success();
        }

        public async Task<OneOf<Success, UserNotFound, NotFound, TestImmutable>> DeleteTestByIdAsync(int testId)
        {
            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            if (DateTime.Now >= test.StartDateTime && DateTime.Now < test.EndDateTime)
            {
                return new TestImmutable("Test has already started");
            }

            await this.testRepository.Delete(test);

            return new Success();
        }

        public async Task<OneOf<Success, ValidationFailed, TestImmutable, NotFound>> 
            UpdateTestQuestionsAsync(int testId, List<TestBaseQuestionDto> testQuestions)
        {
            var validationResult = await this.testBaseQuestionsListValidator.ValidateAsync(testQuestions);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            if (DateTime.Now >= test.StartDateTime)
            {
                return new TestImmutable("Test has already started");
            }

            var testQuestionsList = new TestBaseQuestionsList
            {
                TestBaseId = testId,
                Questions = testQuestions.Select(mapper.Map<Question>).ToList()
            };

            await this.testBaseQuestionsRepository.UpdateQuestionsListAsync(testQuestionsList);

            return new Success();
        }

        public async Task<OneOf<List<TestBaseQuestionDto>, NotFound>> GetTestDraftQuestionsAsync(int testId)
        {
            var result = await this.GetTestBaseQuestionsAsync(testId, this.testDraftRepository);
            return result;
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound>> UpdateTestDraftQuestionsAsync(int testDraftId, List<TestBaseQuestionDto> testQuestions)
        {
            bool testExists = this.testDraftRepository.GetAll().Any(x => x.Id == testDraftId);
            if (!testExists)
            {
                return new NotFound();
            }

            var validationResult = await this.testBaseQuestionsListValidator.ValidateAsync(testQuestions);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var testQuestionsList = new TestBaseQuestionsList
            {
                TestBaseId = testDraftId,
                Questions = testQuestions.Select(mapper.Map<Question>).ToList()
            };

            await this.testBaseQuestionsRepository.UpdateQuestionsListAsync(testQuestionsList);

            return new Success();
        }

        private async Task<OneOf<List<TestBaseQuestionDto>, NotFound>> GetTestBaseQuestionsAsync<T>(
            int testBaseId, 
            IRepository<T> repository)
            where T : TestBase
        {
            bool testExists = repository.GetAll().Any(x => x.Id == testBaseId);
            if (!testExists)
            {
                return new NotFound();
            }

            var questionsList = 
                await this.testBaseQuestionsRepository.GetQuestionsListAsync(testBaseId);

            Console.WriteLine(questionsList is null);

            if (questionsList is null)
            {
                return new List<TestBaseQuestionDto>();
            }

            var questions = questionsList.Questions
                .Select(mapper.Map<TestBaseQuestionDto>).ToList();

            return questions;
        }
    }
}

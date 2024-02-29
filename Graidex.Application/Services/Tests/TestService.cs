using AutoMapper;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.DTOs.Test.TestDraft;
using Graidex.Application.Interfaces;
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
using Graidex.Application.Services.TestChecking.TestCheckingQueue;
using Graidex.Application.Factories.Tests;

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
        private readonly IValidator<CreateTestDraftDto> createTestDraftDtoValidator;
        private readonly IValidator<UpdateTestDraftDto> updateTestDraftDtoValidator;
        private readonly IValidator<CreateTestDto> createTestDtoValidator;
        private readonly IValidator<UpdateTestDto> updateTestDtoValidator;
        private readonly IValidator<UpdateTestTimeDto> updateTestTimeDtoValidator;
        private readonly IValidator<DuplicateTestDraftDto> duplicateTestDraftDtoValidator;
        private readonly IValidator<CreateTestDraftFromTestDto> createTestDraftFromTestDtoValidator;
        private readonly ITestBaseFactory testBaseFactory;

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
            IValidator<List<TestBaseQuestionDto>> testBaseQuestionsListValidator,
            IValidator<CreateTestDraftDto> createTestDraftDtoValidator,
            IValidator<UpdateTestDraftDto> updateTestDraftDtoValidator,
            IValidator<CreateTestDto> createTestDtoValidator,
            IValidator<UpdateTestDto> updateTestDtoValidator,
            IValidator<UpdateTestTimeDto> updateTestTimeDtoValidator
            IValidator<DuplicateTestDraftDto> duplicateTestDraftDtoValidator,
            IValidator<CreateTestDraftFromTestDto> createTestDraftFromTestDtoValidator,
            ITestBaseFactory testBaseFactory)
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
            this.createTestDraftDtoValidator = createTestDraftDtoValidator;
            this.updateTestDraftDtoValidator = updateTestDraftDtoValidator;
            this.createTestDtoValidator = createTestDtoValidator;
            this.updateTestDtoValidator = updateTestDtoValidator;
            this.updateTestTimeDtoValidator = updateTestTimeDtoValidator;
            this.duplicateTestDraftDtoValidator = duplicateTestDraftDtoValidator;
            this.createTestDraftFromTestDtoValidator = createTestDraftFromTestDtoValidator;
            this.testBaseFactory = testBaseFactory;
        }

        public async Task<OneOf<List<TestBaseQuestionDto>, NotFound>> GetTestQuestionsAsync(int testId)
        {
            var result = await this.GetTestBaseQuestionsAsync(testId, this.testRepository);
            return result;
        }

        public async Task<OneOf<GetTestDraftDto, ValidationFailed>> CreateTestDraftForSubjectAsync(int subjectId, CreateTestDraftDto createTestDraftDto)
        {
            var validationResult = this.createTestDraftDtoValidator.Validate(createTestDraftDto);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var testDraft = this.mapper.Map<TestDraft>(createTestDraftDto);

            testDraft.SubjectId = subjectId;
            testDraft.LastUpdate = DateTime.UtcNow;

            await this.testDraftRepository.Add(testDraft);

            // TODO: create questions list

            return this.mapper.Map<GetTestDraftDto>(testDraft);
        }

        public async Task<OneOf<GetTestDraftDto, ValidationFailed, NotFound>> CreateTestDraftFromTestAsync(int testId, CreateTestDraftFromTestDto createTestDraftFromTestDto)
        {
            var validationResult = this.createTestDraftFromTestDtoValidator.Validate(createTestDraftFromTestDto);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            var testDraft = this.testBaseFactory.CreateTestDraft(test);
            testDraft.Title = createTestDraftFromTestDto.Title;
            testDraft.OrderIndex = createTestDraftFromTestDto.OrderIndex;

            await this.testDraftRepository.Add(testDraft);
            
            var questions =
                await this.testBaseQuestionsRepository.GetQuestionsListAsync(testId);
            questions.TestBaseId = testDraft.Id;
            await this.testBaseQuestionsRepository.UpdateQuestionsListAsync(questions);

            return this.mapper.Map<GetTestDraftDto>(testDraft);
        }

        public async Task<OneOf<GetTestDraftDto, ValidationFailed, NotFound>> DuplicateTestDraftAsync(int draftId, DuplicateTestDraftDto duplicateTestDraftDto)
        {
            var validationResult = this.duplicateTestDraftDtoValidator.Validate(duplicateTestDraftDto);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            var clone = this.testBaseFactory.DuplicateTestDraft(testDraft);
            clone.Title = duplicateTestDraftDto.Title;
            clone.OrderIndex = duplicateTestDraftDto.OrderIndex;

            await this.testDraftRepository.Add(clone);

            var questions = 
                await this.testBaseQuestionsRepository.GetQuestionsListAsync(draftId);
            questions.TestBaseId = clone.Id;
            await this.testBaseQuestionsRepository.UpdateQuestionsListAsync(questions);

            return this.mapper.Map<GetTestDraftDto>(clone);
        }

        public async Task<OneOf<GetTestDraftDto, NotFound>> GetTestDraftByIdAsync(int draftId)
        {
            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            return this.mapper.Map<GetTestDraftDto>(testDraft);
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound>> UpdateTestDraftByIdASync(int draftId, UpdateTestDraftDto updateTestDraftDto)
        {
            var validationResult = this.updateTestDraftDtoValidator.Validate(updateTestDraftDto);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            this.mapper.Map(updateTestDraftDto, testDraft);

            testDraft.LastUpdate = DateTime.UtcNow;
            await testDraftRepository.Update(testDraft);

            return new Success();
        }

        public async Task<OneOf<Success, NotFound>> DeleteTestDraftByIdAsync(int draftId)
        {
            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            await this.testDraftRepository.Delete(testDraft);

            return new Success();
        }

        public async Task<OneOf<GetTestDto, ValidationFailed, NotFound, ConditionFailed>> CreateTestFromTestDraftAsync(int draftId, CreateTestDto createTestDto)
        {
            var validationResult = this.createTestDtoValidator.Validate(createTestDto);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var testDraft = await this.testDraftRepository.GetById(draftId);
            if (testDraft is null)
            {
                return new NotFound();
            }

            var questions = 
                await this.testBaseQuestionsRepository.GetQuestionsListAsync(draftId);

            if (questions.Questions.Count < 1)
            {
                return new ConditionFailed("There should be at least one question to create test.");
            }

            var parameters = this.mapper.Map<TestDraftToTestParameters>(createTestDto);
            var test = this.testBaseFactory.CreateTest(testDraft, parameters);

            test.Title = createTestDto.Title;
            test.IsVisible = createTestDto.IsVisible;
            test.OrderIndex = createTestDto.OrderIndex;

            await this.testRepository.Add(test);

            questions.TestBaseId = test.Id;
            await this.testBaseQuestionsRepository.UpdateQuestionsListAsync(questions);

            return this.mapper.Map<GetTestDto>(test);
        }

        public async Task<OneOf<GetTestDto, NotFound>> GetTestByIdAsync(int testId)
        {
            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            return this.mapper.Map<GetTestDto>(test);
        }

        public async Task<OneOf<GetVisibleTestDto, NotFound>> GetVisibleTestByIdAsync(int testId)
        {
            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            return this.mapper.Map<GetVisibleTestDto>(test);
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound>> UpdateTestByIdAsync(int testId, UpdateTestDto updateTestDto)
        {
            var validationResult = this.updateTestDtoValidator.Validate(updateTestDto);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            this.mapper.Map(updateTestDto, test);

            await testRepository.Update(test);

            return new Success();
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound, ItemImmutable>> UpdateTestTimeByIdAsync(int testId, UpdateTestTimeDto updateTestTimeDto)
        {
            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow >= test.StartDateTime)
            {
                return new ItemImmutable("Test has already started");
            }

            var validationResult =
                this.updateTestTimeDtoValidator.Validate(updateTestTimeDto);

            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            this.mapper.Map(updateTestTimeDto, test);

            await testRepository.Update(test);
            return new Success();
        }

        public async Task<OneOf<Success, NotFound, ItemImmutable>> DeleteTestByIdAsync(int testId)
        {
            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow >= test.StartDateTime)
            {
                return new ItemImmutable("Test has already started");
            }

            await this.testRepository.Delete(test);

            return new Success();
        }

        public async Task<OneOf<Success, ValidationFailed, ItemImmutable, NotFound>> 
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

            if (DateTime.UtcNow >= test.StartDateTime)
            { 
                return new ItemImmutable("Test has already started");
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
        
        public async Task<OneOf<Success, NotFound>> AddStudentsToTestAsync(int testId, List<String> studentEmails)
        {   
            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            var subject = await this.subjectRepository.GetById(test.SubjectId);
            if (subject is null)
            {
                return new NotFound();
            }

            var students = this.studentRepository.GetAll().Where(x => studentEmails.Contains(x.Email) && subject.Students.Contains(x) && !test.AllowedStudents.Contains(x));

            foreach (var student in students)
            {
                test.AllowedStudents.Add(student);
            }

            await this.testRepository.Update(test);

            return new Success();
        }

        public async Task<OneOf<Success, NotFound, ItemImmutable>> RemoveStudentsFromTestAsync(int testId, List<String> studentEmails)
        {
            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow >= test.StartDateTime)
            {
                return new ItemImmutable("Test has already started");
            }

            var students = this.studentRepository.GetAll().Where(x => studentEmails.Contains(x.Email) && test.AllowedStudents.Contains(x));
            foreach (var student in students)
            {
                test.AllowedStudents.Remove(student);
            }

            await this.testRepository.Update(test);

            return new Success();
        }
    }
}

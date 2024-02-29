using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.DTOs.Test.TestResult;
using Graidex.Application.Factories.Answers;
using Graidex.Application.Interfaces;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.TestChecking.TestCheckingQueue;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Tests;
using Graidex.Domain.Models.Tests.Answers;
using OneOf;
using OneOf.Types;

namespace Graidex.Application.Services.Tests
{
    public class TestResultService : ITestResultService
    {
        private readonly ICurrentUserService currentUser;
        private readonly IStudentRepository studentRepository;
        private readonly ITestRepository testRepository;
        private readonly ITestResultRepository testResultRepository;
        private readonly ITestBaseQuestionsRepository testBaseQuestionsRepository;
        private readonly ITestResultAnswersRepository testResultAnswersRepository;
        private readonly IAnswerFactory answerFactory;
        private readonly IMapper mapper;
        private readonly IValidator<GetAnswerForStudentDto> getAnswerForStudentDtoValidator;
        private readonly ITestCheckingInQueue testCheckingQueue;
        private readonly ITestResultRecalculationService testResultRecalculationService;
        private const int ExtraMinutesForSubmission = 1;

        public TestResultService(
            ICurrentUserService currentUser,
            IStudentRepository studentRepository,
            ITestRepository testRepository,
            ITestResultRepository testResultRepository,
            ITestBaseQuestionsRepository testBaseQuestionsRepository,
            ITestResultAnswersRepository testResultAnswersRepository,
            IAnswerFactory answerFactory,
            IMapper mapper,
            IValidator<GetAnswerForStudentDto> getAnswerForStudentDtoValidator,
            ITestCheckingInQueue testCheckingQueue,
            ITestResultRecalculationService testResultRecalculationService)
        {
            this.currentUser = currentUser;
            this.studentRepository = studentRepository;
            this.testRepository = testRepository;
            this.testResultRepository = testResultRepository;
            this.testBaseQuestionsRepository = testBaseQuestionsRepository;
            this.testResultAnswersRepository = testResultAnswersRepository;
            this.answerFactory = answerFactory;
            this.mapper = mapper;
            this.getAnswerForStudentDtoValidator = getAnswerForStudentDtoValidator;
            this.testCheckingQueue = testCheckingQueue;
            this.testResultRecalculationService = testResultRecalculationService;
        }

        public async Task<OneOf<GetTestAttemptForStudentDto, UserNotFound, NotFound, ConditionFailed>> StartTestAttemptAsync(int testId)
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

            if (DateTime.UtcNow < test.StartDateTime)
            {
                return new ConditionFailed("Test not started yet");
            }

            if (DateTime.UtcNow > test.EndDateTime)
            {
                return new ConditionFailed("Test is already finished");
            }

            bool attemptAlreadyStarted = this.testResultRepository
                .GetAll()
                .Any(x => x.StudentId == student.Id
                && x.TestId == test.Id);

            if (attemptAlreadyStarted)
            {
                return new ConditionFailed("No more attempts available");
            }

            var startTime = DateTime.UtcNow;
            var endTime = MinDateTime(test.EndDateTime, startTime + test.TimeLimit);

            var testResult = new TestResult
            {
                StartTime = startTime,
                EndTime = endTime,
                TestId = testId,
                StudentId = student.Id
            };

            await this.testResultRepository.Add(testResult);

            // TODO [v1/IMP-3]: Add null check

            var questions = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testId);
            var answers = questions.Questions.Select(this.answerFactory.CreateAnswer).ToList();

            var answersList = new TestResultAnswersList
            {
                TestResultId = testResult.Id,
                Answers = answers
            };

            if (test.ShuffleQuestions)
            {
                ShuffleList(answersList.Answers, testResult.Id);
            }

            await this.testResultAnswersRepository.CreateAnswersListAsync(answersList);

            var testAttemptDto = await this.GetAllQuestionsWithSavedAnswersAsync(testResult.Id);

            if (testAttemptDto.IsT0)
            {
                return testAttemptDto.AsT0;
            }

            return new NotFound();
        }

        private static DateTime MinDateTime(DateTime a, DateTime b)
        {
            return a < b ? a : b;
        }

        private static void ShuffleList<T>(IList<T> list, int? seed = null)
        {
            Random random = seed.HasValue ? new Random(seed.Value) : new Random();

            int n = list.Count;
            T temp;
            while (n > 1)
            {
                int k = random.Next(n);
                n--;
                temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }

        public async Task<OneOf<GetTestAttemptForStudentDto, NotFound, ConditionFailed>> GetAllQuestionsWithSavedAnswersAsync(int testResultId)
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

            if (DateTime.UtcNow > testAttempt.EndTime && !testAttempt.CanReview)
            {
                return new ConditionFailed("The test attempt is already finished");
            }

            var questions
                = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testAttempt.TestId);

            var answers
                = await this.testResultAnswersRepository.GetAnswersListAsync(testResultId);

            var questionsWithAnswers = answers.Answers
                .Select(answer => new GetAnswerDto
                {
                    Question = mapper.Map<TestAttemptQuestionForStudentDto>(questions.Questions[answer.QuestionIndex]),
                    Answer = mapper.Map<GetAnswerForStudentDto>(answer)
                })
                .ToList();

            var testAttemptDto = new GetTestAttemptForStudentDto
            {
                Id = testResultId,
                StartTime = testAttempt.StartTime,
                EndTime = testAttempt.EndTime,
                Answers = questionsWithAnswers,
            };

            return testAttemptDto;
        }

        public async Task<OneOf<Success, NotFound, ItemImmutable, ValidationFailed>> UpdateTestAttemptByIdAsync(int testResultId, int index, GetAnswerForStudentDto answerDto)
        {
            var testAttempt = await this.testResultRepository.GetById(testResultId);
            if (testAttempt is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow > testAttempt.EndTime.AddMinutes(ExtraMinutesForSubmission))
            {
                return new ItemImmutable("This test attempt is already finished");
            }

            var validationResult = this.getAnswerForStudentDtoValidator.Validate(answerDto);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var answerFromDb = await this.testResultAnswersRepository.GetAnswerAsync(testResultId, index);
            if (answerFromDb is null)
            {
                return new NotFound();
            }

            var answer = this.mapper.Map(answerDto, answerFromDb);

            await this.testResultAnswersRepository.UpdateAnswerAsync(testResultId, index, answer);

            return new Success();
        }

        public async Task<OneOf<Success, NotFound>> SubmitTestAttemptByIdAsync(int testResultId)
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

            if (DateTime.UtcNow < testAttempt.EndTime)
            {
                testAttempt.EndTime = DateTime.UtcNow;
            }

            if (test.ReviewResult == Test.ReviewResultOptions.AfterSubmission)
            {
                testAttempt.CanReview = true;
            }

            await this.testResultRepository.Update(testAttempt);

            if (test.AutoCheckAfterSubmission)
            {
                await this.testCheckingQueue.AddAsync(testResultId);
            }

            return new Success();
        }

        public async Task<OneOf<Success, ConditionFailed>> AddTestResultsToCheckingQueueAsync(int testId, IEnumerable<int> testResultIds)
        {
            var allTestResultsOfTest = this.testResultRepository
                .GetAll()
                .Where(x => x.TestId == testId)
                .Select(x => new { id = x.Id, isEnded = x.EndTime <= DateTime.UtcNow })
                .ToList();

            var allTestResultIdsOfTest = allTestResultsOfTest.Select(x => x.id).ToList();

            if (testResultIds.Any(x => !allTestResultIdsOfTest.Contains(x)))
            {
                return new ConditionFailed($"Not all test results belong to the test");
            }

            if (!allTestResultsOfTest.All(x => x.isEnded))
            {
                return new ConditionFailed("Not all test attempts are ended");
            }

            foreach (var testResultId in testResultIds)
            {
                await this.testCheckingQueue.AddAsync(testResultId);
            }

            return new Success();
        }

        public async Task<OneOf<GetTestResultForTeacherDto, NotFound, ConditionFailed>> GetTestResultByIdAsync(int testResultId)
        {
            var testResult = await this.testResultRepository.GetById(testResultId);
            if (testResult is null)
            {
                return new NotFound();
            }

            var student = await this.studentRepository.GetById(testResult.StudentId);
            if (student is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow < testResult.EndTime) 
            {
                return new ConditionFailed("The test attempt is not finished yet");
            }

            var questions
                = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testResult.TestId);

            var answers
                = await this.testResultAnswersRepository.GetAnswersListAsync(testResultId);

            var questionsWithAnswers = answers.Answers
                .Select(answer => new GetResultAnswerForTeacherDto
                {
                    Question = mapper.Map<TestBaseQuestionDto>(questions.Questions[answer.QuestionIndex]),
                    Answer = mapper.Map<GetResultAnswerDto>(answer)
                })
                .ToList();

            var testResultDto = this.mapper.Map<GetTestResultForTeacherDto>(testResult);
            testResultDto.ResultAnswers = questionsWithAnswers;
            testResultDto.StudentEmail = student.Email;

            return testResultDto;
        }

        public async Task<OneOf<Success, NotFound, ItemImmutable>> LeaveFeedBackOnAnswerAsync(int testResultId, int index, LeaveFeedbackForAnswerDto feedbackDto)
        {   
            var testResult = await this.testResultRepository.GetById(testResultId);
            if (testResult is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow < testResult.EndTime)
            {
                return new ItemImmutable("The test attempt is not finished yet");
            }

            // TODO [v1/IMP-3]: Add validation
            // TODO [v1/LG-2]: Add grade and total points recalculation

            await this.testResultAnswersRepository.UpdateAnswerAsync(testResultId, index, mapper.Map<Answer>(feedbackDto));

            await this.testResultRepository.Update(testResult);

            return new Success();
        }

        public async Task<OneOf<GetStudentAttemptsDescriptionDto, UserNotFound, NotFound>> GetStudentAttemptsDescription(int testId)
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

            List<int> submittedTestResultsIds = this.testResultRepository.GetAll()
            .Where(x => x.TestId == test.Id 
                        && x.StudentId == student.Id 
                        && x.EndTime < DateTime.UtcNow)
            .Select(y => y.Id).ToList();

            int? currentTestResultId = this.testResultRepository.GetAll()
            .Where(x => x.TestId == test.Id 
                        && x.StudentId == student.Id 
                        && x.EndTime > DateTime.UtcNow)
            .Select(y => y.Id).FirstOrDefault();

            if (currentTestResultId == 0)
            {
                currentTestResultId = null;
            }

            int numberOfAvailableTestAttempts = 1 - this.testResultRepository.GetAll()
            .Where(x => x.TestId == test.Id && x.StudentId == student.Id)
            .Count();

            GetStudentAttemptsDescriptionDto studentAttemptsDesctiptionDto = new GetStudentAttemptsDescriptionDto
            {
                SubmittedTestResultIds = submittedTestResultsIds,

                CurrentTestResultId = currentTestResultId,

                NumberOfAvailableTestAttempts = numberOfAvailableTestAttempts
            };

            return studentAttemptsDesctiptionDto;
        }
    }
}

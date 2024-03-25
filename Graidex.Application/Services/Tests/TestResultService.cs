using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Test.Answers.TestAttempt;
using Graidex.Application.DTOs.Test.Questions;
using Graidex.Application.DTOs.Test.Questions.QuestionsForStudent;
using Graidex.Application.DTOs.Test.TestAttempt;
using Graidex.Application.DTOs.Test.TestResult;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.Factories.Answers;
using Graidex.Application.Interfaces;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Application.Services.Tests.TestChecking;
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
        private readonly IValidator<List<LeaveFeedbackForAnswerDto>> leaveFeedbackOnAnswerDtoListValidator;
        private readonly ITestCheckingService testCheckingService;
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
            IValidator<List<LeaveFeedbackForAnswerDto>> leaveFeedbackOnAnswerDtoListValidator,
            ITestCheckingService testCheckingService)
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
            this.leaveFeedbackOnAnswerDtoListValidator = leaveFeedbackOnAnswerDtoListValidator;
            this.testCheckingService = testCheckingService;
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

            var questions = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testId);
            var answers = questions.Questions.Select(this.answerFactory.CreateAnswer).ToList();

            var testResult = new TestResult
            {
                StartTime = startTime,
                EndTime = endTime,
                TestId = testId,
                StudentId = student.Id,
                RequireTeacherReview = answers.Any(x => x.RequireTeacherReview),
            };

            await this.testResultRepository.Add(testResult);

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

        private static DateTimeOffset MinDateTime(DateTimeOffset a, DateTimeOffset b)
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

            if (DateTime.UtcNow > testAttempt.EndTime && !testAttempt.ShowToStudent)
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

            var test = await this.testRepository.GetById(testAttempt.TestId);
            if (test is null)
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

            int pointsBefore = answerFromDb.Points;

            var questionFromDb = await this.testBaseQuestionsRepository.GetQuestionAsync(
                testAttempt.TestId, answerFromDb.QuestionIndex);

            var answer = this.mapper.Map(answerDto, answerFromDb);

            testCheckingService.CheckAnswer(questionFromDb, answer);

            testAttempt.TotalPoints += answer.Points - pointsBefore;
            testAttempt.Grade = this.testCheckingService.CalculateGrade(testAttempt.TotalPoints, test.MaxPoints);

            await this.testResultAnswersRepository.UpdateAnswerAsync(testResultId, index, answer);
            await this.testResultRepository.Update(testAttempt);

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

            if (test.ShowToStudent == Test.ShowToStudentOptions.AfterSubmission)
            {
                testAttempt.ShowToStudent = true;
            }

            await this.testResultRepository.Update(testAttempt);

            return new Success();
        }

        public async Task<OneOf<Success, ConditionFailed>> SetShowTestResultsToStudentsAsync(int testId, IEnumerable<int> testResultIds, bool show)
        {
            if (!this.AllTestResultsBelongToTest(testId, testResultIds))
            {
                return new ConditionFailed("Not all test results belong to the test");
            }

            await this.testResultRepository.UpdateShowToStudentAsync(testResultIds, show);
            return new Success();
        }

        public async Task<OneOf<Success, NotFound>> CheckTestResultsWithAIAsync(int testId, IEnumerable<int> testResultIds, CancellationToken cancellationToken)
        {
            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }

            var testResults = this.testResultRepository
                .GetAll()
                .Where(x => testResultIds.Contains(x.Id) && x.EndTime <= DateTimeOffset.Now && x.TestId == testId)
                .ToList();
            
            foreach (var testResult in testResults)
            {
                var questions
                    = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testResult.TestId);

                var answers
                    = await this.testResultAnswersRepository.GetAnswersListAsync(testResult.Id);

                foreach (var answer in answers.Answers)
                {
                    int pointsBefore = answer.Points;
                    var question = questions.Questions[answer.QuestionIndex];
                    await testCheckingService.CheckAnswerWithAI(question, answer, cancellationToken);
                    testResult.TotalPoints += answer.Points - pointsBefore;
                }

                testResult.Grade = this.testCheckingService.CalculateGrade(testResult.TotalPoints, test.MaxPoints);
                await this.testResultAnswersRepository.UpdateAnswersListAsync(new TestResultAnswersList { TestResultId = testResult.Id, Answers = answers.Answers });
                await this.testResultRepository.Update(testResult);
            }

            return new Success();
        }

        private bool AllTestResultsBelongToTest(int testId, IEnumerable<int> testResultIds)
        {
            var allTestResultsOfTest = this.testResultRepository
                .GetAll()
                .Where(x => x.TestId == testId)
                .Select(x => new { id = x.Id })
                .ToList();

            var allTestResultIdsOfTest = allTestResultsOfTest.Select(x => x.id).ToList();

            return testResultIds.Any(x => !allTestResultIdsOfTest.Contains(x));
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
                .Select(answer => new GetResultAnswerForReviewDto
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

        public async Task<OneOf<List<GetTestResultListedForTeacherDto>, NotFound>> GetAllTestResultsByTestIdAsync(int testId) 
        {
            var test = await this.testRepository.GetById(testId);
            if (test is null)
            {
                return new NotFound();
            }
            
            var students = this.studentRepository.GetAll().Where(student => test.AllowedStudents.Contains(student)).ToList();

            var testResults = this.testResultRepository.GetAll().Where(testResult => testResult.TestId == testId).ToList();

            var resultsDtos = testResults.Select(result => new GetTestResultListedForTeacherDto 
            {
                Id = result.Id,
                Student = mapper.Map<StudentInfoDto>(students.Find(student => student.Id == result.StudentId)),
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Grade = result.Grade,
                ShowToStudent = result.ShowToStudent,
                RequireTeacherReview = result.RequireTeacherReview
            }).ToList();

            return resultsDtos;
        }

        public async Task<OneOf<GetTestResultForStudentDto, NotFound, ConditionFailed>> GetTestResultForStudentByIdAsync(int testResultId)
        {
            var testResult = await this.testResultRepository.GetById(testResultId);
            if (testResult is null)
            {
                return new NotFound();
            }
            
            if (testResult.ShowToStudent == false)
            {
                return new ConditionFailed("The test attempt review is not allowed");
            }

            var questions
                = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testResult.TestId);

            var answers
                = await this.testResultAnswersRepository.GetAnswersListAsync(testResultId);

            var questionsWithAnswers = answers.Answers
                .Select(answer => new GetResultAnswerForReviewDto
                {
                    Question = mapper.Map<TestBaseQuestionDto>(questions.Questions[answer.QuestionIndex]),
                    Answer = mapper.Map<GetResultAnswerDto>(answer)
                })
                .ToList();

            var testResultDto = this.mapper.Map<GetTestResultForStudentDto>(testResult);
            testResultDto.ResultAnswers = questionsWithAnswers;

            return testResultDto;
        }

        public async Task<OneOf<Success, ValidationFailed, NotFound, ConditionFailed>> LeaveFeedBackOnAnswerAsync(int testResultId, List<LeaveFeedbackForAnswerDto> feedbackDtos)
        {
            var validationResult = this.leaveFeedbackOnAnswerDtoListValidator.Validate(feedbackDtos);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            var testResult = await this.testResultRepository.GetById(testResultId);
            if (testResult is null)
            {
                return new NotFound();
            }

            if (DateTime.UtcNow < testResult.EndTime)
            {
                return new ConditionFailed("The test attempt is not finished yet");
            }

            var answersList = await this.testResultAnswersRepository.GetAnswersListAsync(testResultId);
            if (answersList is null)
            {
                return new NotFound();
            }

            var questionsList = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testResult.TestId);
            if (questionsList is null)
            {
                return new NotFound();
            }

            foreach (var feedbackDto in feedbackDtos)
            {
                var answer = answersList.Answers.FirstOrDefault(x => x.QuestionIndex == feedbackDto.QuestionIndex);
                if (answer is null)
                {
                    return new NotFound();
                }

                var question = questionsList.Questions[feedbackDto.QuestionIndex];
                if (feedbackDto.Points > question.MaxPoints)
                {
                    return new ConditionFailed("Points cannot be greater than the maximum points for the question");
                }

                var answerWithFeedback = mapper.Map(feedbackDto, answer);
                answer.RequireTeacherReview = false;
            }

            var questions = await this.testBaseQuestionsRepository.GetQuestionsListAsync(testResult.TestId);
            this.testCheckingService.RecalculateTestResultEvaluation(testResult, questions, answersList);
            testResult.RequireTeacherReview = false;

            await this.testResultAnswersRepository.UpdateAnswersListAsync(new TestResultAnswersList { TestResultId = testResultId, Answers = answersList.Answers });

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

            var submittedTestResults = this.testResultRepository.GetAll()
            .Where(x => x.TestId == test.Id 
                        && x.StudentId == student.Id 
                        && x.EndTime < DateTime.UtcNow)
            .ToList();

            var submittedTestResultsDtos 
                = this.mapper.Map<List<GetTestResultPreviewForStudentDto>>(submittedTestResults);

            foreach (var testResultDto in submittedTestResultsDtos)
            {
                if (!testResultDto.ShowToStudent)
                {
                    testResultDto.TotalPoints = null;
                    testResultDto.Grade = null;
                }
            }

            var currentTestResult = this.testResultRepository.GetAll()
            .Where(x => x.TestId == test.Id 
                        && x.StudentId == student.Id 
                        && x.EndTime > DateTime.UtcNow)
            .FirstOrDefault();

            GetTestAttemptPreviewDto? currentTestAttempt = null;
            if (currentTestResult is not null)
            {
                currentTestAttempt 
                    = this.mapper.Map<GetTestAttemptPreviewDto>(currentTestResult);
            }

            int maxNumberOfAttempts = 1;
            int numberOfAvailableTestAttempts = maxNumberOfAttempts - this.testResultRepository.GetAll()
            .Where(x => x.TestId == test.Id && x.StudentId == student.Id)
            .Count();

            var studentAttemptsDescriptionDto = new GetStudentAttemptsDescriptionDto
            {
                SubmittedTestResults = submittedTestResultsDtos,
                CurrentTestAttempt = currentTestAttempt,
                NumberOfAvailableTestAttempts = numberOfAvailableTestAttempts
            };

            return studentAttemptsDescriptionDto;
        }
    }
}

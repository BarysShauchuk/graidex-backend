using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Users;
using Graidex.Domain.Models.Questions;
using Graidex.Domain.Models.Answers;
using Graidex.Domain.Models.ChoiceOptions;

namespace Graidex.Infrastructure.Data
{   
    /// <summary>
    /// Static class with methods used to populate the database with seed data.
    /// </summary>
    public static class SeedData
    {   
        /// <summary>
        /// Applies pending migration to database.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        /// <exception cref="ArgumentNullException">The GraidexDbContext object is null.</exception>
        public static void EnsurePopulated(GraidexDbContext? context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Teachers.Any())
            {
                context.Teachers.AddRange(GetSeedTeachers());
                context.SaveChanges();
            }
        }

        private static List<Teacher> GetSeedTeachers()
        {
            #region Users
            var teacher1 = new Teacher
            {
                Email = "walterwhite@email.com",
                Name = "Walter",
                Surname = "White",
                Password = "12341234",
                Subjects = new List<Subject>()
            };

            var teacher2 = new Teacher
            {
                Email = "someteacher@email.com",
                Name = "Some",
                Surname = "Teacher",
                Password = "pass word",
                Subjects = new List<Subject>()
            };

            var teacher3 = new Teacher
            {
                Email = "butchcassidy@email.com",
                Name = "Butch",
                Surname = "Cassidy",
                Password = "weak$passWo_rd",
                Subjects = new List<Subject>()
            };

            var student1 = new Student
            {
                Email = "jessepinkman@gmail.com",
                Name = "Jesse",
                Surname = "Pinkman",
                Password = "jessepassword",
                CustomId = "IF_200001",
                Subjects = new List<Subject>(),
            };

            var student2 = new Student
            {
                Email = "carelessstudent@gmail.com",
                Name = "Careless",
                Surname = "Student",
                Password = "carelesspassword",
                CustomId = "NS_200002",
                Subjects = new List<Subject>(),
            };

            var student3 = new Student
            {
                Email = "sundancekid@gmail.com",
                Name = "Sundance",
                Surname = "Kid",
                Password = "Str0Ng-Pa$sW0rD",
                CustomId = "NS_200003",
                Subjects = new List<Subject>(),
            };
            #endregion Users

            #region ChoiceOptions
            var choiceOption1 = new ChoiceOption
            {
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
            };

            var choiceOption2 = new ChoiceOption
            {
                Text = "Lorem donec massa sapien faucibus et molestie ac feugiat sed."
            };

            var choiceOption3 = new ChoiceOption
            {
                Text = "Amet volutpat consequat mauris nunc."
            };

            var choiceRecord1 = new ChoiceOptionRecord
            {
                Option = choiceOption1,
                IsCorrect = true
            };

            var choiceRecord2 = new ChoiceOptionRecord
            {
                Option = choiceOption2,
                IsCorrect = false
            };

            var choiceRecord3 = new ChoiceOptionRecord
            {
                Option = choiceOption3,
                IsCorrect = true
            };
            #endregion ChoiceOptions

            #region Questions
            var multQuestion1 = new MultipleChoiceQuestion
            {
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Lorem donec massa sapien faucibus et molestie ac feugiat sed.",
                Options = new List<ChoiceOptionRecord> { choiceRecord1, choiceRecord2, choiceRecord3},
                PointsPerOption = 1,
                MaxPoints = 2
            };

            var multQuestion2 = new MultipleChoiceQuestion
            {
                Text = "Felis eget nunc lobortis mattis.",
                Options = new List<ChoiceOptionRecord> { choiceRecord1, choiceRecord3},
                PointsPerOption = 1,
                MaxPoints = 2
            };

            var multQuestion3 = new MultipleChoiceQuestion
            {
                Text = "Nunc mi ipsum faucibus vitae. Sed nisi lacus sed viverra tellus in hac habitasse platea.",
                Options = new List<ChoiceOptionRecord> { choiceRecord2, choiceRecord3 },
                PointsPerOption = 3,
                MaxPoints = 6
            };

            var openQuestion1 = new OpenQuestion
            {
                Text = "Amet volutpat consequat mauris nunc. Morbi tempus iaculis urna id volutpat lacus.",
                MaxPoints = 3
            };

            var openQuestion2 = new OpenQuestion
            {
                Text = "Viverra maecenas accumsan lacus vel facilisis volutpat est velit egestas.",
                MaxPoints = 2
            };

            var openQuestion3 = new OpenQuestion
            {
                Text = "Massa enim nec dui nunc. Ornare arcu odio ut sem nulla pharetra diam sit.",
                MaxPoints = 1
            };

            var singleQuestion1 = new SingleChoiceQuestion
            {
                Text = "Etiam tempor orci eu lobortis elementum nibh tellus. Sed cras ornare arcu dui vivamus arcu felis bibendum.",
                Options = new List<ChoiceOption> { choiceOption1 },
                CorrectOptionIndex = 0,
                MaxPoints = 1
            };

            var singleQuestion2 = new SingleChoiceQuestion
            {
                Text = "Viverra justo nec ultrices dui sapien eget mi proin. Nunc sed blandit libero volutpat sed cras ornare.",
                Options = new List<ChoiceOption> { choiceOption2, choiceOption3 },
                CorrectOptionIndex = 1,
                MaxPoints = 1
            };

            var singleQuestion3 = new SingleChoiceQuestion
            {
                Text = "Dignissim enim sit amet venenatis urna cursus eget nunc scelerisque.",
                Options = new List<ChoiceOption> { choiceOption1, choiceOption2, choiceOption3 },
                CorrectOptionIndex = 2,
                MaxPoints = 2
            };
            #endregion Questions

            #region Answers
            var multAnswer1 = new MultipleChoiceAnswer
            {
                Question = multQuestion1,
                ChoiceOptionIndexes = new List<int> { 0 },
                Points = 1
            };

            var multAnswer2 = new MultipleChoiceAnswer
            {
                Question = multQuestion2,
                ChoiceOptionIndexes = new List<int> { 0, 2 },
                Points = 2
            };

            var multAnswer3 = new MultipleChoiceAnswer
            {
                Question = multQuestion3,
                ChoiceOptionIndexes = new List<int> { 1, 2 },
                Points = 6
            };

            var openAnswer1 = new OpenAnswer
            {
                Question = openQuestion1,
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                Points = 1
            };

            var openAnswer2 = new OpenAnswer
            {
                Question = openQuestion2,
                Text = "Felis eget nunc lobortis mattis.",
                Points = 0
            };

            var openAnswer3 = new OpenAnswer
            {
                Question = openQuestion3,
                Text = "Nunc mi ipsum faucibus vitae. Sed nisi lacus sed viverra tellus in hac habitasse platea.",
                Points = 0
            };

            var singleAnswer1 = new SingleChoiceAnswer
            {
                Question = singleQuestion1,
                ChoiceOptionIndex = 0,
                Points = 1
            };

            var singleAnswer2 = new SingleChoiceAnswer
            {
                Question = singleQuestion2,
                ChoiceOptionIndex = 1,
                Points = 1
            };

            var singleAnswer3 = new SingleChoiceAnswer
            {
                Question = singleQuestion3,
                ChoiceOptionIndex = 2,
                Points = 2
            };
            #endregion Answers

            #region Subjects
            var subject1 = new Subject
            {
                CustomId = "INF_123",
                Title = "Mathematics",
                Teacher = teacher1,
                Students = new List<Student> { student1, student2 },
                Tests = new List<Test>()
            };

            var subject2 = new Subject
            {
                CustomId = "INF_456",
                Title = "Physics",
                Teacher = teacher2,
                Students = new List<Student> { student2, student3},
                Tests = new List<Test>()
            };

            var subject3 = new Subject
            {
                CustomId = "INF_789",
                Title = "Chemistry",
                Teacher = teacher3,
                Students = new List<Student> { student1, student3},
                Tests = new List<Test>()
            };
            #endregion Subjects

            #region Tests
            var test1 = new Test
            {
                Title = "Math Test 1",
                LastUpdate = DateTime.Now,
                IsHidden = false,
                StartTime = new DateTime(2023, 7, 10, 12, 0, 0),
                EndTime = new DateTime(2023, 7, 10, 13, 0, 0),
                TimeLimit = new TimeSpan(1, 0, 0),
                Subject = subject1,
                AllowedStudents = new List<Student> { student1, student2 },
                Questions = new List<Question> { multQuestion1, openQuestion1, singleQuestion1 },
                GradeToPass = 6,
                Results = new List<TestResult>(),
            };

            var test2 = new Test
            {
                Title = "Physics Test 2",
                LastUpdate = DateTime.Now,
                IsHidden = true,
                StartTime = new DateTime(2023, 6, 10, 12, 0, 0),
                EndTime = new DateTime(2023, 6, 10, 13, 0, 0),
                TimeLimit = new TimeSpan(1, 0, 0),
                Subject = subject2,
                AllowedStudents = new List<Student> { student2, student3 },
                Questions = new List<Question> { multQuestion2, openQuestion2, singleQuestion2, singleQuestion1},
                GradeToPass = 7,
                Results = new List<TestResult>()
            };

            var test3 = new Test
            {
                Title = "Chemistry Test 3",
                LastUpdate = DateTime.Now,
                IsHidden = false,
                StartTime = new DateTime(2023, 5, 10, 12, 0, 0),
                EndTime = new DateTime(2023, 5, 10, 13, 0, 0),
                TimeLimit = new TimeSpan(1, 0, 0),
                Subject = subject3,
                AllowedStudents = new List<Student> { student1, student3 },
                Questions = new List<Question> { multQuestion3, openQuestion3, singleQuestion3, multQuestion2},
                GradeToPass = 8,
                Results = new List<TestResult>()
            };
            #endregion Tests

            #region TestResults
            var testResult1 = new TestResult
            {
                StartTime = new DateTime(2023, 7, 10, 12, 0, 0),
                EndTime = new DateTime(2023, 7, 10, 12, 55, 0),
                Test = test1,
                Student = student1,
                Answers = new List<Answer> {multAnswer1, openAnswer1, singleAnswer1},
            };

            var testResult2 = new TestResult
            {
                StartTime = new DateTime(2023, 6, 10, 12, 0, 0),
                EndTime = new DateTime(2023, 6, 10, 12, 55, 0),
                Test = test2,
                Student = student2,
                Answers = new List<Answer>  {multAnswer2, openAnswer2, singleAnswer2, singleAnswer1},
            };

            var testResult3 = new TestResult
            {
                StartTime = new DateTime(2023, 5, 10, 12, 0, 0),
                EndTime = new DateTime(2023, 5, 10, 12, 55, 0),
                Test = test3,
                Student = student3,
                Answers = new List<Answer> { multAnswer3, openAnswer3, singleAnswer3, multAnswer2},
            };
            #endregion TestResults

            student1.Subjects.Add(subject1);
            student1.Subjects.Add(subject3);

            student2.Subjects.Add(subject1);
            student2.Subjects.Add(subject2);

            student3.Subjects.Add(subject2);
            student3.Subjects.Add(subject3);

            teacher1.Subjects.Add(subject1);
            teacher1.Subjects.Add(subject3);
            teacher2.Subjects.Add(subject2);
            teacher3.Subjects.Add(subject3);
            
            subject1.Tests.Add(test1);
            subject2.Tests.Add(test2);
            subject3.Tests.Add(test3);

            test1.Results.Add(testResult1);
            test2.Results.Add(testResult2);
            test3.Results.Add(testResult3);

            return new List<Teacher> { teacher1, teacher2, teacher3 };
        }
    }
}

﻿using Graidex.Domain.Models.Tests.Answers;
using Graidex.Domain.Models.Tests.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.TestChecking.AnswerCheckers
{
    public class OpenAnswerChecker : AnswerChecker<OpenQuestion, OpenAnswer>
    {
        protected override async Task EvaluateAsync(OpenQuestion question, OpenAnswer answer)
        {
            answer.Feedback = question.DefaultComment;
            answer.Points = 0;

            await Task.Delay(5000);

            // TODO: Implement AI-check here

            //return Task.CompletedTask;
        }
    }
}
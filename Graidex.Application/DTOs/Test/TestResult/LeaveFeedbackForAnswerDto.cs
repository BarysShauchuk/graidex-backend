using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestResult
{
    public class LeaveFeedbackForAnswerDto
    {   
        public int QuestionIndex { get; set; }
        public int Points { get; set; }
        public string? Feedback { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models.Tests.Answers
{
    public class Answer
    {
        public int Points { get; set; }
        public string? Feedback { get; set; }
        public int QuestionIndex { get; set; }
        public virtual bool RequireTeacherReview { get; set; }
    }
}

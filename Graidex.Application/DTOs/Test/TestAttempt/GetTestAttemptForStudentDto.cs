using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestAttempt
{
    public class GetTestAttemptForStudentDto
    {   
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<GetAnswerDto> Answers { get; set; } = new List<GetAnswerDto>();
    }
}

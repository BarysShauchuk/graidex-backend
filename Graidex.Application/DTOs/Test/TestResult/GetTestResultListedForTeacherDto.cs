using Graidex.Application.DTOs.Users.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestResult
{
    public class GetTestResultListedForTeacherDto
    {   
        public required int Id { get; set; }

        public required StudentInfoDto Student { get; set; }

        public required DateTimeOffset StartTime { get; set; }

        public required DateTimeOffset EndTime { get; set;}

        public required int Grade { get; set; }

        public required bool CanReview { get; set; }

    }
}

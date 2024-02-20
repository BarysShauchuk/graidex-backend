﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestResult
{
    public class GetStudentAttemptsDescriptionDto
    {
        public required List<int> SubmittedTestResultIds { get; set; }
        public int? CurrentTestResultId { get; set; }
        public required int NumberOfAvailableTestAttempts { get; set; }
    }
}
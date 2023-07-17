using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.SubjectRequest
{
    public class OutgoingSubjectRequestDto
    {
        public required int Id { get; set; }
        public required string StudentEmail { get; set; }
        public required DateTime Date { get; set; }
    }
}

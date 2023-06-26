using Graidex.Application.DTOs.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.SubjectRequest
{
    public class IncomingSubjectRequestDto
    {
        public int Id { get; set; }
        public required SubjectInfoDto SubjectInfo { get; set; }
        public required DateTime Date { get; set; }
    }
}

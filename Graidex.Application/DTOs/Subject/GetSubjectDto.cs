using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Subject
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public required string CustomId { get; set; }
        public required string Title { get; set; }
        public required string TeacherEmail { get; set; }
    }
}

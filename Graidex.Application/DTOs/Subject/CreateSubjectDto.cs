using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Subject
{
    public class CreateSubjectDto
    {
        public required string CustomId { get; set; }
        public required string Title { get; set; }
    }
}

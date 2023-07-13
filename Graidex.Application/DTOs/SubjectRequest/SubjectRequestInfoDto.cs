using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.SubjectRequest
{
    public class SubjectRequestInfoDto
    {   
        public required string StudentEmail { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required DateTime Date { get; set; }
    }
}

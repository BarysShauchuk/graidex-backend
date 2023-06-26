using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models
{
    public class SubjectRequest
    {
        public int Id { get; set; }
        public required int StudentId { get; set; }
        public required int SubjectId { get; set; }
        public required DateTime Date { get; set; }
    }
}

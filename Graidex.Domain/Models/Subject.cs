using Graidex.Domain.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Graidex.Domain.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [MaxLength(15)]
        public required string CustomId { get; set; }

        [MaxLength(50)]
        public required string Title { get; set; }
        public required virtual Teacher Teacher { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
    }
}

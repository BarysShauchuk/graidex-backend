using System.ComponentModel.DataAnnotations;

namespace Graidex.Domain.Models.Users
{
    public class Student : User
    {
        public int Id { get; set; }

        [MaxLength(15)]
        public string? CustomId { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    }
}

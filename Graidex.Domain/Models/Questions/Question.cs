using System.ComponentModel.DataAnnotations;

namespace Graidex.Domain.Models.Questions
{
    public abstract class Question
    {
        public int Id { get; set; }

        public required virtual Test Test { get; set; }

        public required string Text { get; set; }
    }
}
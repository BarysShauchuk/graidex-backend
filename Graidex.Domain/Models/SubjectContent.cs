using Graidex.Domain.Models.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Models
{
    public class SubjectContent
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the content.
        /// </summary>
        [MaxLength(50)]
        public required string Title { get; set; }

        public bool IsVisible { get; set; }

        public int SubjectId { get; set; }

        public string? ItemType { get; set; }
    }
}

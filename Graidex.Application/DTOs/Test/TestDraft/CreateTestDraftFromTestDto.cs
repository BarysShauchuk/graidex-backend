using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Test.TestDraft
{
    public class CreateTestDraftFromTestDto
    {
        public required string Title { get; set; }
        public double OrderIndex { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.DTOs.Files
{
    public class DownloadFileDto
    {
        public required string FileName { get; set; }
        public required Stream Stream { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.ResultObjects.NonGeneric
{
    public class Failure : Result
    {
        public Failure(string justification)
        {
            Justification = justification;
        }

        public string Justification { get; set; }
    }
}

using Graidex.Application.ResultObjects.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.ResultObjects.NonGeneric
{
    public class ResultFactory
    {
        public Result Success()
        {
            return new Success();
        }

        public Result Failure(string justification = "")
        {
            return new Failure(justification);
        }
    }
}

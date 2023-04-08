using Graidex.Application.ResultObjects.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.ResultObjects.NonGeneric
{
    public abstract class Result
    {
        public bool IsSuccess()
        {
            return this is Success;
        }

        public bool IsFailure(out Failure? failure)
        {
            if (this is Failure failureResult)
            {
                failure = failureResult;
                return true;
            }

            failure = null;
            return false;
        }

        public bool IsFailure()
        {
            return this is Failure;
        }
    }
}

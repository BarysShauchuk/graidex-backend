using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.ResultObjects.Generic
{
    public abstract class Result<T> : IResult<T>
    {
        public bool IsSuccess(out Success<T>? success)
        {
            if (this is Success<T> successResult)
            {
                success = successResult;
                return true;
            }

            success = null;
            return false;
        }

        public bool IsSuccess()
        {
            return this is Success<T>;
        }

        public bool IsFailure(out Failure<T>? failure)
        {
            if (this is Failure<T> failureResult)
            {
                failure = failureResult;
                return true;
            }

            failure = null;
            return false;
        }

        public bool IsFailure()
        {
            return this is Failure<T>;
        }
    }
}

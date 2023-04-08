namespace Graidex.Application.ResultObjects.Generic
{
    public class ResultFactory<T>
    {
        public Result<T> Success(T value)
        {
            return new Success<T>(value);
        }

        public Result<T> Failure(string justification = "")
        {
            return new Failure<T>(justification);
        }
    }
}

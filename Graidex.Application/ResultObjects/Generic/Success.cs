namespace Graidex.Application.ResultObjects.Generic
{
    public class Success<T> : Result<T>
    {
        public Success(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}

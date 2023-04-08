namespace Graidex.Application.ResultObjects.Generic
{
    public class Failure<T> : Result<T>
    {
        public Failure(string justification)
        {
            Justification = justification;
        }

        public string Justification { get; set; }
    }
}

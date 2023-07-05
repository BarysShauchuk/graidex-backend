using System.Runtime.ExceptionServices;

namespace Graidex.API.Extensions
{
    public static class TaskExtensions
    {
        public static async Task WithAggregateException(this Task source)
        {
            try
            {
                await source.ConfigureAwait(false);
            }
            catch
            {
                if (source.Exception is null) throw;
                ExceptionDispatchInfo.Capture(source.Exception).Throw();
            }
        }
    }
}

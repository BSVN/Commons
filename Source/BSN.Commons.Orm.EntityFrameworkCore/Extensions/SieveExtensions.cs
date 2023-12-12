using Sieve.Exceptions;

namespace BSN.Commons.Orm.EntityFrameworkCore.Extensions
{
    /// <summary>
    /// Add some extensions to make Sieve more easy to use
    /// </summary>
    public static class SieveExtensions
    {
        /// <summary>
        /// Ordered extract message from <see cref="SieveException"/> based on inner exceptions
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string ExtractMessage(this SieveException ex)
        {
            string message = ex.InnerException?.InnerException?.Message;

            message = message ?? ex.InnerException?.Message;
            message = message ?? ex.Message;

            return message;
        }
    }
}

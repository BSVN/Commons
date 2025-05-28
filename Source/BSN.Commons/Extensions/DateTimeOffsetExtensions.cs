using System;

namespace BSN.Commons.Extensions
{
    /// <summary>
    /// Provides extension methods for DateTimeOffset.
    /// </summary>
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// Converts a nullable DateTimeOffset to a nullable DateTime.
        /// </summary>
        /// <param name="dateTimeOffset">The DateTimeOffset value to convert.</param>
        /// <returns>A nullable DateTime equivalent, or null if the input is null.</returns>
        public static DateTime? ToDateTimeOrDefault(this DateTimeOffset? dateTimeOffset)
        {
            if (dateTimeOffset.HasValue) {
                return (DateTime?)dateTimeOffset.Value.DateTime;
            }

            return default(DateTime?);
        }
    }
} 
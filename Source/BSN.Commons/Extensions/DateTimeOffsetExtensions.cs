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
        public static DateTime? ToNullableDateTime(this DateTimeOffset? dateTimeOffset)
        {
            return dateTimeOffset?.DateTime;
        }
    }
} 
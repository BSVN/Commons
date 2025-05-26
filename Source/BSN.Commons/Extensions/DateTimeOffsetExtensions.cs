using System;

namespace BSN.Commons.Extensions
{
    // TODO: [BSN-COMMONS] Replace with BSN.Commons nuget package when published to github.com/bsn/commons
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
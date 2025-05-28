using System;

namespace BSN.Commons.Extensions
{
    /// <summary>
    /// Provides extension methods for DateTimeOffset.
    /// </summary>
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// Provides consistent default DateTime values across the project through centralized conversion logic.
        /// </summary>
        /// <remarks>
        /// This extension method provides a standardized way to convert DateTimeOffset to DateTime:
        /// - Maintains consistent null handling across the project
        /// - Optimized for use in LINQ and lambda expressions
        /// - Ensures type-safe conversions in query projections
        /// </remarks>
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
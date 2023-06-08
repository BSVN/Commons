using System;
using System.Globalization;

namespace BSN.Commons.Utilities
{
    // [Todo] R.Noei: Refactor this class to use much less codes for this conversion.
    public static class DateTimeUtilities
    {
        /// <summary>
        /// Convert Georgian Datetime to Shamsi datetime string.
        /// </summary>
        /// <param name="dateTime">Georgian DateTime</param>
        /// <returns>Formatted string of Shamsi DateTime.</returns>
        public static string GetPersianDate(this DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            
            string year = persianCalendar.GetYear(dateTime).ToString();
            string month = persianCalendar.GetMonth(dateTime) < 10 ? $"0{persianCalendar.GetMonth(dateTime)}": persianCalendar.GetMonth(dateTime).ToString();
            string day = persianCalendar.GetDayOfMonth(dateTime) < 10 ? $"0{persianCalendar.GetDayOfMonth(dateTime)}" : persianCalendar.GetDayOfMonth(dateTime).ToString();
            string hour = persianCalendar.GetHour(dateTime) < 10 ? $"0{persianCalendar.GetHour(dateTime)}" : persianCalendar.GetHour(dateTime).ToString();
            string minutes = persianCalendar.GetMinute(dateTime) < 10 ? $"0{persianCalendar.GetMinute(dateTime)}" : persianCalendar.GetMinute(dateTime).ToString();
            string seconds = persianCalendar.GetSecond(dateTime) < 10 ? $"0{persianCalendar.GetSecond(dateTime)}" : persianCalendar.GetSecond(dateTime).ToString();

            return $"{year}/{month}/{day} {hour}:{minutes}:{seconds}";
        }
    }
}

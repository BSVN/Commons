using System;
using System.Globalization;

namespace BSN.Commons.Utilities
{
	public static class TimeUtility
	{
		private static readonly PersianCalendar PersianCalendar;

		static TimeUtility()
		{
			PersianCalendar = new PersianCalendar();
		}

		public static string ToHijriString(this DateTime dateTime)
		{
			return PersianCalendar.GetYear(dateTime).ToString("00") + "/" +
			   PersianCalendar.GetMonth(dateTime).ToString("00") + "/" +
			   PersianCalendar.GetDayOfMonth(dateTime).ToString("00");
		}

		public static DateTime HijriStringToDateTime(this string persiandate)
		{
			var parts = persiandate.Split('/', '-');
			return PersianCalendar.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), 0, 0, 0, 0);
		}
	}
}

using System;

namespace MiNICarRentalBrowser.Extensions
{
	public static class DateTimeExtensions
	{
		public static int YearsElapsed(this DateTime startDate)
		{
			TimeSpan timeSpan = DateTime.Now - startDate;
			return (int)(timeSpan.TotalDays / 365.25);
		}

	}
}

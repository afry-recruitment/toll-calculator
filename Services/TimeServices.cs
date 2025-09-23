using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tollCalculator.Data;
using tollCalculator.Models;

namespace tollCalculator.Services
{
	public class TimeServices
	{
		private readonly ApplicationDbContext _dbContext;

		public TimeServices(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

		}

		public DateTime GetCurrentDateTime()
		{
			return DateTime.Now;
		}

		//public DateTime getCurrentDateTime()
		//{
		//	DateTime julafton = new DateTime(2025, 12, 24, 14, 3, 0);

		//	return julafton;
		//}

		//public DateTime getCurrentDateTime()
		//{
		//	var sunday = new DateTime(2025, 9, 21);

		//	return sunday;
		//}

		//public DateTime getCurrentDateTime()
		//{
		//	var rushHour = new DateTime(2025, 9, 24, 7, 15, 0);
		//	var rushHour = new DateTime(2025, 9, 24, 17, 08, 0);

		//	return rushHour;
		//}

		public bool IsHoliday(DateTime date, List<DateTime> holidayList)
		{
			foreach (var holiday in holidayList)
			{
				if (date.Date == holiday.Date)
				{
					return true;
				}

			}
			return false;
		}

		public bool IsWeekend(DateTime date)
		{
			string weekDay = date.DayOfWeek.ToString();

			if (weekDay is "Saturday" || weekDay is "Sunday")
				return true;

			else
				return false;
		}

		public bool IsRushHour(DateTime now)
		{
			//Rush hour between 7:00 - 7:59 and 15:30 - 16:59
			if ((now.TimeOfDay >= new TimeSpan(7, 0, 0) &&
				now.TimeOfDay <= new TimeSpan(7, 59, 59)) ||
				(now.TimeOfDay >= new TimeSpan(15, 30, 0) &&
				now.TimeOfDay <= new TimeSpan(16, 59, 59)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsDuringNoTollHours(DateTime now)
		{
			//Within the interval 00:00 - 05:59:59
			TimeSpan morningStart = new TimeSpan(0, 0, 0);
			TimeSpan morningEnd = new TimeSpan(5, 59, 59);

			//Within the interval 18:30 - 23:59:59
			TimeSpan eveningStart = new TimeSpan(18, 30, 0);
			TimeSpan eveningEnd = new TimeSpan(23, 59, 59);

			TimeSpan currentTime = now.TimeOfDay;

			return (currentTime >= morningStart && currentTime <= morningEnd) ||
				   (currentTime >= eveningStart && currentTime <= eveningEnd);
		}

	}
}

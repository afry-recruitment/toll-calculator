using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tollCalculator.Services
{
	public class TimeServices
	{
		public DateTime getCurrentDateTime()
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

		public bool isHoliday(DateTime date, List<DateTime> holidayList)
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

		public bool isWeekEnd(DateTime date)
		{
			string weekDay = date.DayOfWeek.ToString();

			if (weekDay is "Saturday" || weekDay is "Sunday")
				return true;

			else
				return false;
		}

		public bool isRushHour(DateTime date)
		{
			if ((date.TimeOfDay >= new TimeSpan(6, 30, 0) &&
				date.TimeOfDay <= new TimeSpan(8, 30, 0)) ||
				(date.TimeOfDay >= new TimeSpan(15, 30, 0) &&
				date.TimeOfDay <= new TimeSpan(17, 30, 0)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

	
				


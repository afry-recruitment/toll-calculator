package calculations;

import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

/**
 * 
 * @author danijela
 * 
 *Calculate is the day holiday (for 2022) /weekend for actual year
 *If it is not holiday/weekend it returns false otherwise it return true
 */

public class IsTollFreeDate {

	public Boolean isTollFreeDate(Date date) {
		Calendar calendar = GregorianCalendar.getInstance();
		calendar.setTime(date);
		int year = calendar.get(Calendar.YEAR);
		int month = calendar.get(Calendar.MONTH);
		int day = calendar.get(Calendar.DAY_OF_MONTH);

		int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);

		if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) {
			return true;
		} else if (year == 2022) {
			if (month == Calendar.JANUARY && day == 6 
					|| month == Calendar.APRIL && (day == 15 || day == 18)
					|| month == Calendar.MAY && (day == 26 || day == 8 || day == 9)
					|| month == Calendar.JUNE && (day == 6 || day == 24) 
					|| month == Calendar.DECEMBER && (day == 26))
			{
				return true;
			}
		}
		return false;

	}

}

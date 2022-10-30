import java.util.Calendar;
import java.util.Date;

public class CheckIfNoFeeDate {
	
	  //Could not find any good way to implement how to check if its a swedish holiday for every year, so i made a version for 2022.
	  //I  do not like this solution at all, should make a dynamic version that works for every year instead.
	  static Boolean isTollFreeDate(Date date, Calendar calendar) {
		    calendar.setTime(date);
		    int year = calendar.get(Calendar.YEAR);
		    int month = calendar.get(Calendar.MONTH);
		    int day = calendar.get(Calendar.DAY_OF_MONTH);

		    int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
		    if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) return true;

		    if (year == 2013) {
		      if (month == Calendar.JANUARY && (day == 1 || day == 6) ||
		          month == Calendar.APRIL && (day == 15 || day == 17 || day == 18) || 
		          month == Calendar.MAY && (day == 1 || day == 26) ||
		          month == Calendar.JUNE && (day == 5 || day == 6 || day == 25) ||
		          month == Calendar.NOVEMBER && day == 5 ||
		          month == Calendar.DECEMBER && (day == 25 || day == 26)) {
		        return true;
		      }
		    }
		    return false;
		  }
}

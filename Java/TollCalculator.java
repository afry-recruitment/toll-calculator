

import java.text.ParseException;
import java.time.LocalDate;
import java.time.ZoneId;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.concurrent.TimeUnit;

public class TollCalculator {
	
	public static final int MAX_FEE=60;
  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
 * @throws ParseException 
   */
  public int getTollFee(Vehicle vehicle, Date... dates) throws ParseException {    
    if(null!=dates && null!=vehicle){
        Arrays.sort(dates);
        Date intervalStart = dates[0];
        int totalFee = 0;
        for (Date date : dates) {
          if (totalFee > MAX_FEE) {
            totalFee = MAX_FEE;
            return totalFee;
          }
          int nextFee = getTollFee(date, vehicle);
          int tempFee = getTollFee(intervalStart, vehicle);

          TimeUnit timeUnit = TimeUnit.MINUTES;
          long diffInMillies = date.getTime() - intervalStart.getTime();
          long minutes = timeUnit.convert(diffInMillies, TimeUnit.MILLISECONDS);

          if (minutes <= 60) {
            if (totalFee > 0) totalFee -= tempFee;
            if (nextFee >= tempFee) tempFee = nextFee;
            totalFee += tempFee;
          } else {
            totalFee += nextFee;
            intervalStart = date;
          }
        }
        return totalFee;
      }else{
        throw new RuntimeException("Vehicle or Dates should not be null");
      }
  }

  private boolean isTollFreeVehicle(Vehicle vehicle) {
    if(vehicle == null) return false;
    return vehicle.isTollFreeVechicle();
  }

  public int getTollFee( Date date, Vehicle vehicle) throws ParseException {
	  
    if(isTollFreeDate(date) || isWeekend( date) || isTollFreeVehicle(vehicle))    return 0;
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    int hour = calendar.get(Calendar.HOUR_OF_DAY);
    int minute = calendar.get(Calendar.MINUTE);

    if (((hour == 6 || (hour == 18)) && minute >= 0 && minute <= 29)
    	|| (hour >= 8 && hour <= 14 && minute >= 0 && minute <= 59))
    {
    	return 8;
    }
    else if ((((hour == 6) || (hour == 17)) && minute >= 30 && minute <= 59)
    	|| (hour == 17 && minute >= 0 && minute <= 59))
    {
    	return 13;
    }
    else if (((hour == 7) || (hour == 15) || (hour == 16) && minute >= 0 && minute <= 59))
    {
    	return 18;
    }
    else return 0;
  }

  /**
   * check public holidays
   * @param date
   * @return boolean
   */
  private Boolean isTollFreeDate(Date date) {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    PublicHolidays publicHoliday = new PublicHolidays(Calendar.YEAR);
    LocalDate localDate = date.toInstant().atZone(ZoneId.systemDefault()).toLocalDate();
    return publicHoliday.isPublicHoliday(localDate);
  }
  
  /***
   * check weekends
   * @param date
   * @return boolean
   */
 public boolean isWeekend( Date date){
	
	    Calendar calender = Calendar.getInstance();
	    calender.setTime(date);
	    if ((calender.get(Calendar.DAY_OF_WEEK) == Calendar.SATURDAY) || (calender.get(Calendar.DAY_OF_WEEK) == Calendar.SUNDAY))
	    {   
	   return true;
	 
	    }
	return false;
	  
  }
 
}


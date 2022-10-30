import java.util.*;
import java.util.concurrent.*;

public class TollCalculator {

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  private final Calendar calendar = GregorianCalendar.getInstance();
  private final int MAX_FEE = 60;
  
	
  public int getTollFee(Vehicle vehicle, Date... dates) {
    Date intervalStart = dates[0];
    int totalFee = 0;
    for (Date date : dates) {      
      int nextFee = getTollFeeCost(date, vehicle);
      int tempFee = getTollFeeCost(intervalStart, vehicle);
      
      long difference = date.getTime() - intervalStart.getTime();
      long minutes = TimeUnit.MILLISECONDS.toMinutes(difference);

      if (minutes <= 60) {
        if (totalFee > 0) totalFee -= tempFee;
        if (nextFee >= tempFee) tempFee = nextFee;
        totalFee += tempFee;
      } else {
        totalFee += nextFee;
      }
      if (totalFee >= 60) return MAX_FEE;
    }
    return totalFee;
  }
  
  public int getTollFeeCost(final Date date, Vehicle vehicle) {  
	    if(CheckIfNoFeeDate.isTollFreeDate(date, calendar) || Vehicle.isTollFreeVehicle(vehicle)) return 0;
	    calendar.setTime(date);
	    int hour = calendar.get(Calendar.HOUR_OF_DAY);
	    int minute = calendar.get(Calendar.MINUTE);
	    if(rushHour(date)) return 18;
	    if (((hour == 6 || hour == 18) && minute <= 29) || (hour == 8 && minute >= 30)) return 8;
	    else if (hour == 6 || hour == 8 || hour == 17) return 13;
	    
	    return 0;
  }
  
  public boolean rushHour(Date date) {
	  calendar.setTime(date);
	  int hour = calendar.get(Calendar.HOUR_OF_DAY);
	  if(hour == 7 || hour == 15 || hour == 16) {
		  return true;
	  }
	  return false;
  }
}


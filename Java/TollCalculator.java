import java.util.*;
import java.util.concurrent.*;

import Exception.DateException;
import Vehicles.Vehicle;

public class TollCalculator {
  // year for calculations
  public Integer calcYear = 2023;

  // maximum toll for the day
  public Integer maxDailyToll = 60;

  // minimum minutes between each toll
  public Integer minTimeBetweenTolls = 60;

  // set values for different time fees
  public Integer freeTimeFee = 0;
  public Integer lowTrafficFee = 8;
  public Integer mediumTrafficFee = 13;
  public Integer rushHourFee = 18;

  public void setCalcYear(Integer year) {
    this.calcYear = year;
  }

  // Traffic situation for sorting of toll fee times (mainly for future implementations of non-static values)
  public enum Traffic{
    FREETIME,
    LOW,
    MEDIUM,
    RUSHHOUR
  }

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   * @throws DateException
   */
  public int getTollFee(Vehicle vehicle, Date... dates) throws DateException {

    //Check our input variables for nullpointers or empty arrays
    if (vehicle == null)
      throw new NullPointerException("No vehicle");
    if (dates == null || dates.length < 1)
      throw new NullPointerException("Date not set");
    
    //Check if the dates really are the same day
    if (!getIsSameDates(dates))
      throw new DateException("Dates are for different days");

    Date intervalStart = dates[0];
    int totalFee = 0;

    //Skip calculations for toll free vehicles and return totalFee = 0
    if (vehicle.isTollFree())
      return totalFee;

    //Skip calculations for toll free days and return totalFee = 0
    if (isTollFreeDate(dates[0]))
      return totalFee;

    for (Date date : dates) {
      int nextFee = getTollFee(date, vehicle);
      int tempFee = getTollFee(intervalStart, vehicle);

      TimeUnit timeUnit = TimeUnit.MINUTES;
      long diffInMillies = date.getTime() - intervalStart.getTime();
      long minutes = timeUnit.convert(diffInMillies, TimeUnit.MILLISECONDS);

      if (minutes <= minTimeBetweenTolls) {
        if (totalFee > 0) totalFee -= tempFee;
        if (nextFee >= tempFee) tempFee = nextFee;
        totalFee += tempFee;
      } else {
        totalFee += nextFee;
      }
    }
    if (totalFee > maxDailyToll) totalFee = maxDailyToll;
    return totalFee;
  }

  // Test if any of the dates varies in the dates array
  public boolean getIsSameDates(final Date... dates) {
    Calendar calendar1 = GregorianCalendar.getInstance();
    Calendar calendar2 = GregorianCalendar.getInstance();

    for (int i=0; i < dates.length; i++) {
      for (int k=0; k < dates.length; k++) {
          calendar1.setTime(dates[i]);
          calendar2.setTime(dates[k]);
          
          if(calendar1.get(Calendar.DAY_OF_MONTH) != calendar2.get(Calendar.DAY_OF_MONTH))
            return false;
      }
    }
    return true;
  }

  public int getTollFee(final Date date, Vehicle vehicle) throws DateException {

    Traffic FeeTime = Traffic.FREETIME;
    FeeTime = getFeeTime(date);

    switch(FeeTime) {
      case FREETIME:
        return freeTimeFee;
      case LOW:
        return lowTrafficFee;
      case MEDIUM:
        return mediumTrafficFee;
      case RUSHHOUR:
        return rushHourFee;
      default:
        throw new DateException("Fee Time is not in any category, FREETIME, LOW, MEDIUM or RUSHHOUR");
    }
  }

  // Getter for traffic situation, should be improved further to non static values
  private Traffic getFeeTime(Date date)
  {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    int hour = calendar.get(Calendar.HOUR_OF_DAY);
    int minute = calendar.get(Calendar.MINUTE);

    // PROBLEMATIC PART WAY TOO HARDCODED
    // Should be sorted another way, maybe with maps. Because this is messy, but I dont know how :)
    if      (hour == 6 && minute >= 0 && minute <= 29) return Traffic.LOW; // 06:00 - 06:29
    else if (hour == 6 && minute >= 30 && minute <= 59) return Traffic.MEDIUM; // 06:30 - 06:59
    else if (hour == 7 && minute >= 0 && minute <= 59) return Traffic.RUSHHOUR; // 07:00 - 07:59
    else if (hour == 8 && minute >= 0 && minute <= 29) return Traffic.MEDIUM; // 08:00 - 08:29
    else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return Traffic.LOW; // 08:30 - 14:59
    else if (hour == 15 && minute >= 0 && minute <= 29) return Traffic.MEDIUM; // 15:00 - 15:29
    else if (hour == 15 && minute >= 30 || hour == 16 && minute <= 59) return Traffic.RUSHHOUR; // 15:30 - 16:59
    else if (hour == 17 && minute >= 0 && minute <= 59) return Traffic.MEDIUM; // 17:00 - 17:59
    else if (hour == 18 && minute >= 0 && minute <= 29) return Traffic.LOW; // 18:00 - 18:29
    else return Traffic.FREETIME;
  }

  private Boolean isTollFreeDate(Date date) throws DateException {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    int year = calendar.get(Calendar.YEAR);
    int month = calendar.get(Calendar.MONTH);
    int day = calendar.get(Calendar.DAY_OF_MONTH);

    int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
    if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) return true;

    if (year == calcYear) {
      // PROBLEMATIC PART WAY TOO HARDCODED
      // This should also be sorted better, with another handling class referencing a map.
      // Holidays https://www.kalenderdagar.se/roda-dagar/roda-dagar-2023/
      if (month == Calendar.JANUARY && (day == 1 || day == 6) ||
          month == Calendar.APRIL && (day == 7 || day == 8 || day == 9 || day == 10) ||
          month == Calendar.MAY && (day == 1 || day == 18) ||
          month == Calendar.JUNE && day == 6 ||
          month == Calendar.JULY ||
          month == Calendar.DECEMBER && (day == 25 || day == 26)) {
        return true;
      }
    } else throw new DateException("Data is from another year than intended calculations");
    return false;
  }
}


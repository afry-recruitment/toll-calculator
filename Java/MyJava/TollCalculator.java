package MyJava;

import java.util.*;
import java.util.concurrent.*;
import java.util.Date;

public class TollCalculator {

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   * @throws Exception
   */
  public int getTollFeeTotal(Vehicle vehicle, Date... dates) throws Exception {
    
    int totalFee = 0;
    int nextFee = 0;
    Date startTime = dates[0];
    long minuteCounter = 0;
    boolean startTimerSet = false;

    // Check that all dates passed are concerning the same day
    Calendar calStart = Calendar.getInstance(TimeZone.getTimeZone("GMT+1:00"));
    calStart.setTime(dates[0]);
    int yearStart = calStart.get(Calendar.YEAR);
    int monthStart= calStart.get(Calendar.MONTH);
    int dayStart = calStart.get(Calendar.DAY_OF_MONTH);
    boolean justOneDay = true;
    for (Date dateCheck : dates) {
      Calendar cal = Calendar.getInstance(TimeZone.getTimeZone("GMT+1:00"));
      cal.setTime(dateCheck);
      if (cal.get(Calendar.YEAR) != yearStart || cal.get(Calendar.MONTH) != monthStart || cal.get(Calendar.DAY_OF_MONTH) != dayStart) {
        justOneDay = false;
      } 
    }
    if (!justOneDay) throw new Exception("The dates passed concern more than one day!!!!!");
    
    // Calculate total toll fee
    for (Date date : dates) {
      int tempFee = getTollFee(date, vehicle);

      if (tempFee == 0) {
        totalFee += nextFee;
        nextFee = 0;
        minuteCounter = 0;
  
      } else {
  
        if (startTimerSet == false) {
          startTime = date;
          startTimerSet = true;
        } else {
          TimeUnit timeUnit = TimeUnit.MINUTES;
          long diffInMillies = date.getTime() - startTime.getTime();
          minuteCounter = timeUnit.convert(diffInMillies, TimeUnit.MILLISECONDS);
        }

        if (minuteCounter > 60) {
          if (date == dates[dates.length -1]) {
            totalFee += tempFee += nextFee;
            break;
          }
          totalFee += nextFee;
          nextFee = 0;
          minuteCounter = 0;
          startTime = date;
        } 

        if (tempFee > nextFee) nextFee = tempFee;
        if (date == dates[dates.length - 1]) {
          totalFee += nextFee;;
          break;
        }
      }
    }

    if (totalFee > 60) totalFee = 60;
    return totalFee;
  }

  private boolean isTollFreeVehicle(Vehicle vehicle) {
    if(vehicle == null) return false;
    String vehicleType = vehicle.getType();
    return vehicleType.equals(TollFreeVehicles.MOTORBIKE.getType()) ||
           vehicleType.equals(TollFreeVehicles.TRACTOR.getType()) ||
           vehicleType.equals(TollFreeVehicles.EMERGENCY.getType()) ||
           vehicleType.equals(TollFreeVehicles.DIPLOMAT.getType()) ||
           vehicleType.equals(TollFreeVehicles.FOREIGN.getType()) ||
           vehicleType.equals(TollFreeVehicles.MILITARY.getType());
  }

  public int getTollFee(final Date date, Vehicle vehicle) {
    if(isTollFreeDate(date) || isTollFreeVehicle(vehicle)) return 0;
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    int hour = calendar.get(Calendar.HOUR_OF_DAY);
    int minute = calendar.get(Calendar.MINUTE);

    if (hour == 6 && minute >= 0 && minute <= 29) return 8;
    else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
    else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
    else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
    else if (hour == 8 && minute >= 30 && minute <= 59 || hour >= 9 && hour <= 14 || hour == 14 && minute >= 0 && minute <= 59) return 8;
    else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
    else if (hour == 15 && minute >= 30 && minute <= 59 || hour == 16 && minute >= 0 && minute <= 59) return 18;
    else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
    else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
    else return 0;
  }

  private Boolean isTollFreeDate(Date date) {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    int year = calendar.get(Calendar.YEAR);
    int month = calendar.get(Calendar.MONTH);
    int day = calendar.get(Calendar.DAY_OF_MONTH);

    int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
    if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) return true;

    if (year == 2013) {
      if (month == Calendar.JANUARY && day == 1 ||
          month == Calendar.MARCH && (day == 28 || day == 29) ||
          month == Calendar.APRIL && (day == 1 || day == 30) ||
          month == Calendar.MAY && (day == 1 || day == 8 || day == 9) ||
          month == Calendar.JUNE && (day == 5 || day == 6 || day == 21) ||
          month == Calendar.JULY ||
          month == Calendar.NOVEMBER && day == 1 ||
          month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31)) {
        return true;
      }
    }
    return false;
  }

  private enum TollFreeVehicles {
    MOTORBIKE("Motorbike"),
    TRACTOR("Tractor"),
    EMERGENCY("Emergency"),
    DIPLOMAT("Diplomat"),
    FOREIGN("Foreign"),
    MILITARY("Military");
    private final String type;

    TollFreeVehicles(String type) {
      this.type = type;
    }

    public String getType() {
      return type;
    }
  }
}

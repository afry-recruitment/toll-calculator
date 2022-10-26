package service;

import model.TollFreeVehicles;
import model.Vehicle;

import java.time.Instant;
import java.time.LocalDateTime;
import java.time.ZoneId;
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

  RushHoursHub rushHoursHub = new RushHoursHub();

  

  public int getTollFee(Vehicle vehicle, final LocalDateTime... dates) {
    LocalDateTime intervalStart = dates[0];
    int totalFee = 0;
    for (LocalDateTime date : dates) {
      int nextFee = rushHoursHub.getTollFeeAtPeakTimesCalculus(date, vehicle);
      int tempFee = rushHoursHub.getTollFeeAtPeakTimesCalculus(intervalStart, vehicle);

      TimeUnit timeUnit = TimeUnit.MINUTES;
      long diffInMillis = localDateTimeToDate(date).getTime() - localDateTimeToDate(intervalStart).getTime();
      long minutes = timeUnit.convert(diffInMillis, TimeUnit.MILLISECONDS);

      if (minutes <= 60) {
        if (totalFee > 0) totalFee -= tempFee;
        if (nextFee >= tempFee) tempFee = nextFee;
        totalFee += tempFee;
      } else {
        totalFee += nextFee;
      }
    }
    if (totalFee > 60) totalFee = 60;
    return totalFee;
  }

  public boolean isTollFreeVehicle(Vehicle vehicle) {
    if(vehicle == null) return false;
    String vehicleType = vehicle.getType();
    return vehicleType.equals(TollFreeVehicles.MOTORBIKE.getType()) ||
           vehicleType.equals(TollFreeVehicles.TRACTOR.getType()) ||
           vehicleType.equals(TollFreeVehicles.EMERGENCY.getType()) ||
           vehicleType.equals(TollFreeVehicles.DIPLOMAT.getType()) ||
           vehicleType.equals(TollFreeVehicles.FOREIGN.getType()) ||
           vehicleType.equals(TollFreeVehicles.MILITARY.getType());
  }


  public Boolean isTollFreeDate(LocalDateTime date) {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(localDateTimeToCalendar(date));
    int year = calendar.get(Calendar.YEAR);
    int month = calendar.get(Calendar.MONTH);
    int day = calendar.get(Calendar.DAY_OF_MONTH);

    int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
    if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) return true;

    // change the logic of the year to use a lib or implement
    if (year == 2013) {
      return month == Calendar.JANUARY && day == 1 ||
              month == Calendar.MARCH && (day == 28 || day == 29) ||
              month == Calendar.APRIL && (day == 1 || day == 30) ||
              month == Calendar.MAY && (day == 1 || day == 8 || day == 9) ||
              month == Calendar.JUNE && (day == 5 || day == 6 || day == 21) ||
              month == Calendar.JULY ||
              month == Calendar.NOVEMBER && day == 1 ||
              month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31);
    }
    return false;
  }



  public static Date localDateTimeToDate(final LocalDateTime dateTime) {
    final Instant instant = dateTime.atZone(ZoneId.systemDefault())
            .toInstant();
    return Date.from(instant);
  }

  public static Date localDateTimeToCalendar(LocalDateTime date) {
    LocalDateTime localDateTime = LocalDateTime.now();
    Instant instant = localDateTime.atZone(ZoneId.systemDefault()).toInstant();
    return Date.from(instant);
  }

}


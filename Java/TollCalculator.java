
import java.util.*;
import java.util.concurrent.*;

public class TollCalculator {

  /*
   * we can define constants for the toll fees and toll-free dates
   */

  private static final int FEE_MINUTE_0_29 = 8;
  private static final int FEE_MINUTE_30_59 = 13;
  private static final int FEE_HOUR_7 = 18;
  private static final int FEE_HOUR_6 = 13;
  private static final int FEE_HOUR_8_TO_14 = 8;
  private static final int FEE_HOUR_15 = 13;
  private static final int FEE_HOUR_16_TO_17 = 18;
  private static final int FEE_HOUR_18 = 8;
  private static final int MAX_DAILY_FEE = 60;

  private static final Set<Integer> TOLL_FREE_DATES = Set.of(
      // Dates that are toll-free, represented as integers in the format YYYYMMDD
      20130101, 20130328, 20130329, 20130401, 20130430,
      20130501, 20130508, 20130509, 20130605, 20130606,
      20130621, 20130701, 20131101, 20131224, 20131225,
      20131226, 20131231
  );

  private static final Set<String> TOLL_FREE_VEHICLE_TYPES = Set.of(
      // Types of vehicles that are toll-free
      "Motorbike", "Tractor", "Emergency", "Diplomat", "Foreign", "Military"
  );

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getTollFee(Vehicle vehicle, Date... dates) {
    Date intervalStart = dates[0];
    int totalFee = 0;
    for (Date date : dates) {
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

  /*
   * Now we can replace the hardcoded values in the getTollFee method with the constants:
   */

  public int getTollFee(final Date date, Vehicle vehicle) {
    if(isTollFreeDate(date) || isTollFreeVehicle(vehicle)) return 0;
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    int hour = calendar.get(Calendar.HOUR_OF_DAY);
    int minute = calendar.get(Calendar.MINUTE);

    if (hour == 6 && minute >= 0 && minute <= 29) return FEE_MINUTE_0_29;
    else if (hour == 6 && minute >= 30 && minute <= 59) return FEE_MINUTE_30_59;
    else if (hour == 7 && minute >= 0 && minute <= 59) return FEE_HOUR_7;
    else if (hour == 8 && minute >= 0 && minute <= 29) return FEE_HOUR_8_TO_14;
    else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return FEE_HOUR_8_TO_14;
    else if (hour == 15 && minute >= 0 && minute <= 29) return FEE_HOUR_15;
    else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return FEE_HOUR_16_TO_17;
    else if (hour == 17 && minute >= 0 && minute <= 59) return FEE_HOUR_16_TO_17;
    else if (hour == 18 && minute >= 0 && minute <= 29) return FEE_HOUR_18;
    else return 0;
}

/*
 * each array contains the year, month, and day of a toll-free date. The code then iterates over the list and checks if the input date matches any of the toll-free dates in the list. If a match is found, the function returns true. If no match is found, the function returns false.
 */

private Boolean isTollFreeDate(Date date) {
  Calendar calendar = GregorianCalendar.getInstance();
  calendar.setTime(date);
  int year = calendar.get(Calendar.YEAR);
  int month = calendar.get(Calendar.MONTH);
  int day = calendar.get(Calendar.DAY_OF_MONTH);

  int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
  if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) {
      return true;
  }

  List<Integer[]> tollFreeDates = new ArrayList<Integer[]>();
  tollFreeDates.add(new Integer[] {2013, Calendar.JANUARY, 1});
  tollFreeDates.add(new Integer[] {2013, Calendar.MARCH, 28});
  tollFreeDates.add(new Integer[] {2013, Calendar.MARCH, 29});
  tollFreeDates.add(new Integer[] {2013, Calendar.APRIL, 1});
  tollFreeDates.add(new Integer[] {2013, Calendar.APRIL, 30});
  tollFreeDates.add(new Integer[] {2013, Calendar.MAY, 1});
  tollFreeDates.add(new Integer[] {2013, Calendar.MAY, 8});
  tollFreeDates.add(new Integer[] {2013, Calendar.MAY, 9});
  tollFreeDates.add(new Integer[] {2013, Calendar.JUNE, 5});
  tollFreeDates.add(new Integer[] {2013, Calendar.JUNE, 6});
  tollFreeDates.add(new Integer[] {2013, Calendar.JUNE, 21});
  tollFreeDates.add(new Integer[] {2013, Calendar.JULY, 1});
  tollFreeDates.add(new Integer[] {2013, Calendar.NOVEMBER, 1});
  tollFreeDates.add(new Integer[] {2013, Calendar.DECEMBER, 24});
  tollFreeDates.add(new Integer[] {2013, Calendar.DECEMBER, 25});
  tollFreeDates.add(new Integer[] {2013, Calendar.DECEMBER, 26});
  tollFreeDates.add(new Integer[] {2013, Calendar.DECEMBER, 31});

  for (Integer[] tollFreeDate : tollFreeDates) {
      if (year == tollFreeDate[0] && month == tollFreeDate[1] && day == tollFreeDate[2]) {
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


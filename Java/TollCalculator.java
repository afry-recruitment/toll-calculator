import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.time.Month;
import java.time.MonthDay;
import java.time.temporal.ChronoUnit;
import java.util.*;

public class TollCalculator {

  private static final List<HolidayPeriod> holidays = createHolidays();
  private static final List<TollPeriod> tollPeriods = createTollPeriods();

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getTollFee(Vehicle vehicle, LocalDateTime... dates) {
    if (dates == null || dates.length == 0) {
      return 0;
    }

    LocalDateTime intervalStart = dates[0];
    int totalFee = 0;

    for (int i = 0; i < dates.length; i++) {
      LocalDateTime date = dates[i];
      int nextFee = getTollFee(date, vehicle);
      int tempFee = getTollFee(intervalStart, vehicle);

      long minutesSinceIntervalStart = ChronoUnit.MINUTES.between(intervalStart, date);

      if (minutesSinceIntervalStart < 60) {
        if (totalFee > 0)
          totalFee -= tempFee;
        if (nextFee >= tempFee)
          tempFee = nextFee;
        totalFee += tempFee;
      } else {
        totalFee += nextFee;
      }
    }

    return Math.min(totalFee, 60);
  }

  public int getTollFee(final LocalDateTime date, Vehicle vehicle) {
    if (isTollFreeDate(date) || isTollFreeVehicle(vehicle))
      return 0;

    for (TollPeriod tollPeriod : tollPeriods) {
      if (tollPeriod.contains(date.toLocalTime())) {
        return tollPeriod.getFee();
      }
    }

    return 0;
  }

  private static List<HolidayPeriod> createHolidays() {
    List<HolidayPeriod> holidays = new ArrayList<>();

    holidays.add(new HolidayPeriod(Month.DECEMBER, 24, Month.DECEMBER, 26));
    holidays.add(new HolidayPeriod(Month.DECEMBER, 31, Month.JANUARY, 1));
    holidays.add(new HolidayPeriod(Month.MARCH, 28, Month.MARCH, 29));
    holidays.add(new HolidayPeriod(Month.APRIL, 1));
    holidays.add(new HolidayPeriod(Month.APRIL, 30, Month.MAY, 1));
    holidays.add(new HolidayPeriod(Month.MAY, 8, Month.MAY, 9));
    holidays.add(new HolidayPeriod(Month.JUNE, 5, Month.JUNE, 6));
    holidays.add(new HolidayPeriod(Month.JUNE, 21));
    holidays.add(new HolidayPeriod(Month.JULY, 1, Month.JULY, 31));
    holidays.add(new HolidayPeriod(Month.NOVEMBER, 1));

    return holidays;
  }

  private boolean isHoliday(LocalDateTime date) {
    for (HolidayPeriod holidayPeriod : holidays) {
      if (holidayPeriod.contains(MonthDay.from(date.toLocalDate()))) {
        return true;
      }
    }
    return false;
  }

  private boolean isTollFreeDate(LocalDateTime date) {
    DayOfWeek dayOfWeek = date.getDayOfWeek();
    if (dayOfWeek == DayOfWeek.SATURDAY || dayOfWeek == DayOfWeek.SUNDAY)
      return true;

    return isHoliday(date);
  }

  public enum Vehicle {
    MOTORBIKE,
    TRACTOR,
    EMERGENCY,
    DIPLOMAT,
    FOREIGN,
    MILITARY,

    TOLL_FREE_ABOVE, // Toll free vehicles must be above this enum

    CAR;
  }

  private boolean isTollFreeVehicle(Vehicle vehicle) {
    if (vehicle == null) {
      throw new IllegalArgumentException("Vehicle cannot be null");
    }

    if (vehicle == Vehicle.TOLL_FREE_ABOVE) {
      throw new IllegalArgumentException("Vehicle cannot be TOLL_FREE_ABOVE");
    }
    return vehicle.compareTo(Vehicle.TOLL_FREE_ABOVE) < 0;
  }

  private static List<TollPeriod> createTollPeriods() {
    List<TollPeriod> tollPeriods = new ArrayList<>();

    tollPeriods.add(new TollPeriod(6, 0, 6, 29, 8));
    tollPeriods.add(new TollPeriod(6, 30, 6, 59, 13));
    tollPeriods.add(new TollPeriod(7, 0, 7, 59, 18));
    tollPeriods.add(new TollPeriod(8, 0, 8, 29, 13));
    tollPeriods.add(new TollPeriod(15, 0, 15, 29, 13));
    tollPeriods.add(new TollPeriod(15, 30, 15, 59, 18));
    tollPeriods.add(new TollPeriod(16, 0, 16, 59, 18));
    tollPeriods.add(new TollPeriod(17, 0, 17, 59, 13));
    tollPeriods.add(new TollPeriod(18, 0, 18, 29, 8));

    for (int hour = 8; hour < 14; hour++) {
      tollPeriods.add(new TollPeriod(hour, 30, hour, 59, 8));
    }

    return tollPeriods;

  }
}

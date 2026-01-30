
import java.time.*;
import java.util.*;
import java.util.concurrent.*;

public class TollCalculator {

  public int getTollFee(Vehicle vehicle, Date... dates) {
    if (dates == null || dates.length == 0) {
      return 0;
    }

    List<Date> sortedDates = new ArrayList<>(Arrays.asList(dates));
    sortedDates.sort(Comparator.comparingLong(Date::getTime));

    LocalDate firstDay = toLocalDate(sortedDates.get(0));
    for (Date date : sortedDates) {
      if (!toLocalDate(date).equals(firstDay)) {
        throw new IllegalArgumentException("All dates must be on the same day");
      }
    }

    Date intervalStart = sortedDates.get(0);
    int totalFee = 0;
    int tempFee = getTollFee(intervalStart, vehicle);
    totalFee += tempFee;

    for (int i = 1; i < sortedDates.size(); i++) {
      Date date = sortedDates.get(i);
      int nextFee = getTollFee(date, vehicle);

      long diffInMillies = date.getTime() - intervalStart.getTime();

      if (diffInMillies <= TimeUnit.HOURS.toMillis(1)) {
        if (nextFee > tempFee) {
          totalFee += (nextFee - tempFee);
          tempFee = nextFee;
        }
      } else {
        totalFee += nextFee;
        intervalStart = date;
        tempFee = nextFee;
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
    else if ((hour == 8 && minute >= 30) || (hour >= 9 && hour <= 14)) return 8;
    else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
    else if ((hour == 15 && minute >= 30) || (hour == 16 && minute <= 59)) return 18;
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

    LocalDate localDate = LocalDate.of(year, month + 1, day);
    return getTollFreeDates(year).contains(localDate);
  }

  private Set<LocalDate> getTollFreeDates(int year) {
    Set<LocalDate> dates = new HashSet<>();

    dates.add(LocalDate.of(year, Month.JANUARY, 1));
    dates.add(LocalDate.of(year, Month.JANUARY, 6));
    dates.add(LocalDate.of(year, Month.MAY, 1));
    dates.add(LocalDate.of(year, Month.JUNE, 6));
    dates.add(LocalDate.of(year, Month.DECEMBER, 24));
    dates.add(LocalDate.of(year, Month.DECEMBER, 25));
    dates.add(LocalDate.of(year, Month.DECEMBER, 26));
    dates.add(LocalDate.of(year, Month.DECEMBER, 31));

    LocalDate easterSunday = getEasterSunday(year);
    dates.add(easterSunday.minusDays(2));
    dates.add(easterSunday);
    dates.add(easterSunday.plusDays(1));
    dates.add(easterSunday.plusDays(39));
    dates.add(easterSunday.plusDays(49));

    LocalDate midsummerEve = getMidsummerEve(year);
    dates.add(midsummerEve);
    dates.add(midsummerEve.plusDays(1));

    for (int day = 1; day <= Month.JULY.length(Year.isLeap(year)); day++) {
      dates.add(LocalDate.of(year, Month.JULY, day));
    }

    return dates;
  }

  private LocalDate toLocalDate(Date date) {
    return Instant.ofEpochMilli(date.getTime())
        .atZone(ZoneId.systemDefault())
        .toLocalDate();
  }

  private LocalDate getMidsummerEve(int year) {
    LocalDate june19 = LocalDate.of(year, Month.JUNE, 19);
    int daysUntilFriday = (DayOfWeek.FRIDAY.getValue() - june19.getDayOfWeek().getValue() + 7) % 7;
    return june19.plusDays(daysUntilFriday);
  }

  private LocalDate getEasterSunday(int year) {
    int a = year % 19;
    int b = year / 100;
    int c = year % 100;
    int d = b / 4;
    int e = b % 4;
    int f = (b + 8) / 25;
    int g = (b - f + 1) / 3;
    int h = (19 * a + b - d - g + 15) % 30;
    int i = c / 4;
    int k = c % 4;
    int l = (32 + 2 * e + 2 * i - h - k) % 7;
    int m = (a + 11 * h + 22 * l) / 451;
    int month = (h + l - 7 * m + 114) / 31;
    int day = ((h + l - 7 * m + 114) % 31) + 1;
    return LocalDate.of(year, month, day);
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


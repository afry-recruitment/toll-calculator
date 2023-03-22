
<<<<<<< HEAD
=======
import vehicle.Vehicle;
>>>>>>> 36e9c27 (Fixing Code)
import java.time.DayOfWeek;
import java.time.Duration;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.Month;
import java.time.temporal.TemporalAdjusters;
import java.util.*;

public class TollCalculator {

  private static final Set<String> tollFreeVehicleTypes = Set.of("Motorbike", "Tractor", "Emergency", "Diplomat", "Foreign", "Military");

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param passes   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getTollFee(Vehicle vehicle, LocalDateTime... passes) {
    if (passes == null || passes.length == 0|| passes[0] == null) {
      throw new RuntimeException("No passes were given");
    }
    checkIfAllPassesIsTheSameDay(passes);
    LocalDateTime lastHighestIntervall = null;
    int lastHighestFee = 0;
    int totalFee = 0;
    for (LocalDateTime date : passes) {
      if (totalFee >= 60) {
        break;
      }

      int nextFee = getFeeOnePass(date, vehicle);

      if (lastHighestIntervall == null || Duration.between(lastHighestIntervall, date).toMinutes() > 60) {
        lastHighestFee = nextFee;
        lastHighestIntervall = date;
        totalFee += nextFee;

      } else {
        if (nextFee > lastHighestFee) {
          lastHighestFee = nextFee;
          lastHighestIntervall = date;
          totalFee += lastHighestFee;
        }
      }
    }
    if (totalFee > 60) totalFee = 60;
    return totalFee;
  }

  private void checkIfAllPassesIsTheSameDay(LocalDateTime... passes) {
    LocalDateTime firstPass = passes[0];
    LocalDate firstPassDate = firstPass.toLocalDate();

    for (int i = 1; i < passes.length; i++) {
      LocalDateTime currentPass = passes[i];
      LocalDate currentPassDate = currentPass.toLocalDate();

      if (!currentPassDate.equals(firstPassDate)) {
        throw new RuntimeException("Passes are on different days");
      }
    }
  }

  private boolean isTollFreeVehicle(Vehicle vehicle) {
    if (vehicle == null) return false;

    return tollFreeVehicleTypes.contains(vehicle.getType());
  }

  public int getFeeOnePass(final LocalDateTime date, Vehicle vehicle) {
    if(isTollFreeDate(date) || isTollFreeVehicle(vehicle)) return 0;
    int hour = date.getHour();
    int minute = date.getMinute();


    if (hour == 6 && minute >= 30 ) return 13;
    else if (hour == 7 ) return 18;
    else if (hour == 8 && minute <= 29) return 13;
    else if (hour == 15 && minute <= 29) return 13;
    else if (hour == 15 && minute >= 30 || hour == 16) return 18;
    else if (hour == 17) return 13;
    else return 8;
  }

  private Boolean isTollFreeDate(LocalDateTime date) {
    int year = date.getYear();

    DayOfWeek dayOfWeek = date.getDayOfWeek();
    if (dayOfWeek == DayOfWeek.SATURDAY || dayOfWeek == DayOfWeek.SUNDAY) return true;
    LocalDate EasterSunday = calculateEaster(year);

    Set<LocalDate> specialDates = new HashSet<>(Arrays.asList(
            LocalDate.of(year, Month.JANUARY, 1),
            LocalDate.of(year, Month.APRIL, 30),
            LocalDate.of(year, Month.MAY, 1),
            LocalDate.of(year, Month.JUNE, 5),
            LocalDate.of(year, Month.JUNE, 6),
            LocalDate.of(year, Month.DECEMBER, 24),
            LocalDate.of(year, Month.DECEMBER, 25),
            LocalDate.of(year, Month.DECEMBER, 26),
            LocalDate.of(year, Month.DECEMBER, 31),
            EasterSunday.minusDays(3), //Maundy Thursday
            EasterSunday.minusDays(2), //Good friday
            EasterSunday.plusDays(1), //Easter Monday
            EasterSunday.plusDays(39), //Ascension day
            calculateDateOfFridays(year, 24), //Midsummer
            calculateDateOfFridays(year, 43) //All Saints Day

    ));

    return specialDates.contains(date.toLocalDate());
  }

  private LocalDate calculateEaster(int year) {
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

  private LocalDate calculateDateOfFridays(int year, int week) {
    LocalDate firstDayOfYear = LocalDate.of(year, 1, 1);
    LocalDate firstFriday = firstDayOfYear.with(TemporalAdjusters.nextOrSame(DayOfWeek.FRIDAY));
    return firstFriday.plusWeeks(week);
  }
}


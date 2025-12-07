package afry.tollcollector.logic;

import afry.tollcollector.domain.Holidays;
import afry.tollcollector.domain.TollFees;
import afry.tollcollector.domain.TollFreeVehicleTypes;
import afry.tollcollector.domain.Vehicle;
import java.util.*;
import java.util.function.Function;
import java.util.stream.Collectors;
import java.util.stream.Stream;
import org.springframework.stereotype.Service;

@Service
public class TollCalculator {

  private Holidays holidays;
  private TollFees tollFees;
  private TollFreeVehicleTypes tollFreeVehicleTypes;

  public TollCalculator(
      Holidays holidays, TollFees tollFees, TollFreeVehicleTypes tollFreeVehicleTypes) {
    this.holidays = holidays;
    this.tollFees = tollFees;
    this.tollFreeVehicleTypes = tollFreeVehicleTypes;
  }

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getTollFee(Vehicle vehicle, Date... dates) {
    if (isTollFreeVehicle(vehicle) || dates.length == 0 || isTollFreeDate(dates[0])) {
      return 0;
    }
    Function<Date, Integer> getHour =
        d -> {
          Calendar calendar = GregorianCalendar.getInstance();
          calendar.setTime(d);
          return calendar.get(Calendar.HOUR_OF_DAY);
        };
    // TODO: specification mentions same hour period; verify that this naive implementation - which
    // chunks on every full hour rather than on hour-segments - is the correct interpretation
    Map<Integer, List<Date>> hourGroupedDates =
        Stream.of(dates).collect(Collectors.groupingBy(getHour));

    int calculatedFee =
        hourGroupedDates.values().stream()
            .mapToInt(
                s ->
                    s.stream()
                        .mapToInt(this::getTollFee)
                        .max()
                        .orElseThrow(() -> new IllegalStateException("Could not get toll fees")))
            .sum();

    return Integer.min(60, calculatedFee);
  }

  private boolean isTollFreeVehicle(Vehicle vehicle) {
    if (vehicle == null) return false;
    return tollFreeVehicleTypes.isTollFreeVehicleType(vehicle);
  }

  int getTollFee(final Date date) {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    int hour = calendar.get(Calendar.HOUR_OF_DAY);
    int minute = calendar.get(Calendar.MINUTE);

    return tollFees.getTollFees(hour, minute);
  }

  private boolean isWeekend(Calendar calendar) {
    int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
    return (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY);
  }

  private boolean isHoliday(int year, int month, int day) {
    return holidays.isHolidayIfKnown(year, month, day);
  }

  private Boolean isTollFreeDate(Date date) {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    if (isWeekend(calendar)) return true;
    int year = calendar.get(Calendar.YEAR);
    int month = calendar.get(Calendar.MONTH);
    int day = calendar.get(Calendar.DAY_OF_MONTH);

    return isHoliday(year, month, day);
  }

}

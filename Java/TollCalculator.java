import java.time.*;
import java.time.temporal.ChronoUnit;
import java.util.*;

public class TollCalculator {

  private static final List<FeeTimeRange> TIME_RANGES = List.of(
    FeeTimeRange.of(6, 0, 6, 29, 8),
    FeeTimeRange.of(6, 30, 6, 59, 13),
    FeeTimeRange.of(7, 0, 7, 59, 18),
    FeeTimeRange.of(8, 0, 8, 29, 13),
    FeeTimeRange.of(8, 30, 8, 59, 8),
    FeeTimeRange.of(9, 30, 9, 59, 8),
    FeeTimeRange.of(10, 30, 10, 59, 8),
    FeeTimeRange.of(11, 30, 11, 59, 8),
    FeeTimeRange.of(12, 30, 12, 59, 8),
    FeeTimeRange.of(13, 30, 13, 59, 8),
    FeeTimeRange.of(14, 30, 14, 59, 8),
    FeeTimeRange.of(15, 0, 15, 29, 13),
    FeeTimeRange.of(15, 30, 16, 59, 18),
    FeeTimeRange.of(17, 0, 17, 59, 13),
    FeeTimeRange.of(18, 0, 18, 30, 8)
  );

  private static final List<DateRange> HOLLIDAYS = List.of(
    DateRange.of(1, 1, 1, 1),
    DateRange.of(3, 28, 3, 29),
    DateRange.of(4, 1, 4, 1),
    DateRange.of(4, 30, 4, 30),
    DateRange.of(5, 1, 5, 1),
    DateRange.of(5, 8, 5, 9),
    DateRange.of(6, 5, 6, 6),
    DateRange.of(6, 21, 6, 21),
    DateRange.of(7, 1, 7, 31),
    DateRange.of(11, 1, 11, 1),
    DateRange.of(12, 24, 12, 26),
    DateRange.of(12, 31, 12, 31)
  );

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getDailyFee(Vehicle vehicle, LocalDateTime... datetimes) {
    
    if (vehicle.isTollFree() || datetimes.length == 0)
      return 0;

    List<LocalDateTime> sortedDates = Arrays.stream(datetimes).sorted().toList();

    LocalDateTime intervalStart = sortedDates.get(0);
    int totalFee = 0;

    int maxFeeInWindow = getTollFee(intervalStart, vehicle);

    for (int i = 1; i < sortedDates.size(); i++) {
      LocalDateTime datetime = sortedDates.get(i);

      long diffInMinutes = ChronoUnit.MINUTES.between(intervalStart, datetime);

      int currentFee = getTollFee(datetime, vehicle);

      if (diffInMinutes <= 60) {
        maxFeeInWindow = Math.max(maxFeeInWindow, currentFee);
      } else {
        totalFee += maxFeeInWindow;
        intervalStart = datetime;
        maxFeeInWindow = currentFee;
      }
    }

    // Add the last fee
    totalFee += maxFeeInWindow;

    if (totalFee > 60) totalFee = 60;
    return totalFee;
  }

  /**
   * Determine the toll fee for a specific pass
   *
   * @param datetime - date and time of the pass
   * @param vehicle - the vehicle
   * @return - the fee for the pass
   */
  public int getTollFee(LocalDateTime datetime, Vehicle vehicle) {
    if(isTollFreeDate(datetime.toLocalDate()) || vehicle.isTollFree()) 
      return 0;
    
    LocalTime time = datetime.toLocalTime();

    for (FeeTimeRange range : TIME_RANGES) {
      if (range.contains(time))
        return range.getFee();
    }

    return 0;
  }

  
  private Boolean isTollFreeDate(LocalDate date) {
    DayOfWeek dayOfWeek = date.getDayOfWeek();
    if (dayOfWeek == DayOfWeek.SATURDAY || dayOfWeek == DayOfWeek.SUNDAY) return true;

    MonthDay md = MonthDay.from(date);

    for (DateRange range : HOLLIDAYS) {
      if (range.contains(md))
        return true;
    }

    return false;
  }

}


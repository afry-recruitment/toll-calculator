import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.MonthDay;
import java.util.*;

public class TollCalculator {

    private static final NavigableMap<LocalTime, Integer> FEE_SCHEDULE = initializeFeeSchedule();
    /**
     * Calculate the total toll fee for one day.
     * Assumes all passages belong to the same calendar date
     * and are provided in chronological order.
     *
     * @param vehicle   the vehicle being charged
     * @param passages  date and time of all passes on one day
     * @return the total toll fee for that day (max 60 SEK)
     */
  // Calculate the total toll fee for one day with LocalDateTime
  public int getTollFee(Vehicle vehicle, List<LocalDateTime> passages) {
      if (vehicle == null || passages == null || passages.isEmpty()) {
          return 0;
      }

      LocalDateTime intervalStart = passages.getFirst();

      int totalFee = 0;
      int maxFeeInWindow = feePerPassage(intervalStart, vehicle);

      for (int i = 1; i< passages.size(); i++) {

          LocalDateTime currentPassage = passages.get(i);

          int currentFee = feePerPassage(currentPassage, vehicle);
          long minutesDifference = java.time.Duration.between(intervalStart, currentPassage).toMinutes();

          if(minutesDifference <= 60) {
              if (currentFee > maxFeeInWindow)
                  maxFeeInWindow = currentFee;
          }
          else {
              totalFee = totalFee + maxFeeInWindow;
              intervalStart = currentPassage;
              maxFeeInWindow = currentFee;
          }
      }
      totalFee = totalFee + maxFeeInWindow;

      if (totalFee > 60)
          totalFee = 60;

      return totalFee;
  }

  private boolean isTollFreeVehicle(Vehicle vehicle) {
    return vehicle != null && vehicle.isTollFree();
  }

  // Calculate fee for a single passage
  private int feePerPassage(LocalDateTime passage, Vehicle vehicle) {
      LocalDate date = passage.toLocalDate();
      LocalTime time = passage.toLocalTime();

      if (isTollFreeDate(date) || isTollFreeVehicle(vehicle))
          return 0;

      return getFeeForTime(time);
  }
  // Get fee based on time using the fee schedule map
  private int getFeeForTime(LocalTime time) {
      Map.Entry<LocalTime, Integer> entry = FEE_SCHEDULE.floorEntry(time);
      return entry != null ? entry.getValue() : 0;
  }

  // Initialize the fee schedule map, a cleaner way to set up the time-fee pairs and get rid of repetitive and inefficient if else statements
  private static NavigableMap<LocalTime, Integer> initializeFeeSchedule() {
    NavigableMap<LocalTime, Integer> feeSchedule = new TreeMap<>();
      feeSchedule.put(LocalTime.of(6, 0), 8);
      feeSchedule.put(LocalTime.of(6, 30), 13);
      feeSchedule.put(LocalTime.of(7, 0), 18);
      feeSchedule.put(LocalTime.of(8, 0), 13);
      feeSchedule.put(LocalTime.of(8, 30), 8);
      feeSchedule.put(LocalTime.of(15, 0), 13);
      feeSchedule.put(LocalTime.of(15, 30), 18);
      feeSchedule.put(LocalTime.of(17, 0), 13);
      feeSchedule.put(LocalTime.of(18, 0), 8);
      feeSchedule.put(LocalTime.of(18, 30), 0);
      return feeSchedule;
  }
  // Check if the date is toll-free (weekends or holidays)
  private boolean isTollFreeDate(LocalDate date) {
      boolean weekend = date.getDayOfWeek().getValue() >= 6;
      boolean holiday = TOLL_FREE_HOLIDAYS.contains(MonthDay.from(date));

      return weekend || holiday;
  }

  // Define a set of fixed toll-free holidays, more holidays can be added as needed first parameter is month, second is day
  private static final Set<MonthDay> TOLL_FREE_HOLIDAYS = Set.of(
      MonthDay.of(1, 1),   // New Year's Day
      MonthDay.of(5, 1),   // Labour Day
      MonthDay.of(12, 25), // Christmas Day
      MonthDay.of(12, 31)  // New Year's Eve
  );

}


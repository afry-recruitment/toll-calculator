
import java.time.LocalDate;
import java.time.MonthDay;
import java.util.*;
import java.util.concurrent.*;
import java.time.LocalDateTime;
import java.time.ZoneId;
import java.util.stream.Collectors;

public class TollCalculator {

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getTollFee(Vehicle vehicle, Date... dates) {
      if (vehicle == null || dates == null || dates.length == 0) {
          return 0;
      }

      if (isTollFreeVehicle(vehicle)) {
          return 0;
      }

      List<LocalDateTime> passages = Arrays.stream(dates)
              .map(d -> LocalDateTime.ofInstant(d.toInstant(), ZoneId.systemDefault()))
              .collect(Collectors.toList());

      return calculateDailyFee(vehicle, passages);
  }
  // Calculate the total toll fee for one day with LocalDateTime
  public int calculateDailyFee(Vehicle vehicle, List<LocalDateTime> passages) {
      /*if (vehicle == null || passages == null || passages.isEmpty()) {
          return 0;
      }*/

      LocalDateTime intervalStart = passages.get(0);
      int totalFee = 0;

      int maxFeeInWindow = getTollFee(Date.from(intervalStart.atZone(ZoneId.systemDefault()).toInstant()), vehicle);

      for (int i = 1; i< passages.size(); i++) {

          LocalDateTime currentPassage = passages.get(i);

          int currentFee = getTollFee(Date.from(currentPassage.atZone(ZoneId.systemDefault()).toInstant()), vehicle);
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
    if(vehicle == null)
        return false;

    VehicleType type = mapVehicleType(vehicle.getType());
    return type.isTollFree();
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
    else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
    else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
    else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
    else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
    else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
    else return 0;
  }

  private Boolean isTollFreeDate(LocalDate date) {
      boolean weekend = date.getDayOfWeek().getValue() >= 6;
      boolean holiday = TOLL_FREE_HOLIDAYS.contains(MonthDay.from(date));

      return weekend || holiday;
  }

  private static final Set<MonthDay> TOLL_FREE_HOLIDAYS = Set.of(
      MonthDay.of(1, 1),   // New Year's Day
      MonthDay.of(5, 1),   // Labour Day
      MonthDay.of(12, 25), // Christmas Day
      MonthDay.of(12, 31)  // New Year's Eve
  );

  // Maps string representation of vehicle type to VehicleType enum
  private VehicleType mapVehicleType(String type) {
      if(type == null)
          throw new IllegalArgumentException("Vehicle type cannot be null");
      return switch (type.toUpperCase()) {
          case "CAR" -> VehicleType.CAR;
          case "MOTORBIKE" -> VehicleType.MOTORBIKE;
          case "TRACTOR" -> VehicleType.TRACTOR;
          case "EMERGENCY" -> VehicleType.EMERGENCY;
          case "DIPLOMAT" -> VehicleType.DIPLOMAT;
          case "FOREIGN" -> VehicleType.FOREIGN;
          case "MILITARY" -> VehicleType.MILITARY;
          case "TRUCK" -> VehicleType.TRUCK;
          case "BUS" -> VehicleType.BUS;
          default -> throw new IllegalArgumentException("Unknown vehicle type: " + type);
      };
  }
}


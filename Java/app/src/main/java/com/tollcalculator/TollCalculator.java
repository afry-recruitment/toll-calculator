package com.tollcalculator;

import java.time.LocalTime;
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
  public int getTollFee(Vehicle vehicle, Date... dates) {
    List<Charge> feesForTheDay = new ArrayList<Charge>();

    Arrays.sort(dates);

    for (int i = 0; i < dates.length; i++) {
      Date currentIterationDate = dates[i];

      if (i == 0) {
        // First iteration
        Charge charge = new Charge(currentIterationDate, getTollFee(currentIterationDate, vehicle));
        feesForTheDay.add(charge);
      } else {

        // Error handling to catch if we receive dates on multiple different days
        Date previousIterationDate = dates[i - 1];

        if(!isDatesOnTheSameDate(currentIterationDate, previousIterationDate)){
          throw new Error("The dates must be on the same day!");
        }

        // Check if more than 60 minutes has passed since the last charge
        Charge lastCharge = feesForTheDay.get(feesForTheDay.size() - 1);

        long timePassedInMillis = currentIterationDate.getTime() - lastCharge.getTimestamp().getTime();
        long timePassedInMinutes = TimeUnit.MINUTES.convert(timePassedInMillis, TimeUnit.MILLISECONDS);

        if (timePassedInMinutes > 60) {
          Charge charge = new Charge(currentIterationDate, getTollFee(currentIterationDate, vehicle));
          feesForTheDay.add(charge);
        }
      }
    }

    int chargeSum = 0;

    for (Charge charge : feesForTheDay) {
      chargeSum += charge.getCost();
    }

    return (chargeSum > 60 ? 60 : chargeSum);
  }

  private boolean isDatesOnTheSameDate(Date date1, Date date2){
    Calendar cal1 = Calendar.getInstance();
    Calendar cal2 = Calendar.getInstance();

    cal1.setTime(date1);
    cal2.setTime(date2);

    boolean sameYear = cal1.get(Calendar.YEAR) == cal2.get(Calendar.YEAR);
    boolean sameMonth = cal1.get(Calendar.MONTH) == cal2.get(Calendar.MONTH);
    boolean sameDay = cal1.get(Calendar.DAY_OF_MONTH) == cal2.get(Calendar.DAY_OF_MONTH);

    return sameYear && sameMonth && sameDay;
  }

  public boolean isTollFreeVehicle(Vehicle vehicle) {
    if (vehicle == null) return false;

    for (TollFreeVehicles tollFreeVehicleValue : TollFreeVehicles.values()) {
      if (tollFreeVehicleValue.getType().equals(vehicle.getType())) {
        return true;
      }
    }

    return false;
  }

  public int getTollFee(final Date date, Vehicle vehicle) {
    if (isTollFreeDate(date) || isTollFreeVehicle(vehicle))
      return 0;

    // Create a list of all intervals
    List<TimeFee> timeFees = new ArrayList<TimeFee>();

    timeFees.add(new TimeFee("06:00", "06:29", 8)); // 06:00 - 06:29 -> 8
    timeFees.add(new TimeFee("06:30", "06:59", 13)); // 06:30 - 06:59 -> 13
    timeFees.add(new TimeFee("07:00", "07:59", 18)); // 07:00 - 07:59 -> 18
    timeFees.add(new TimeFee("08:00", "08:29", 13)); // 08:00 - 08:29 -> 13
    timeFees.add(new TimeFee("08:30", "14:59", 8)); // 08:30 - 14:59 -> 8
    timeFees.add(new TimeFee("15:00", "15:29", 13)); // 15:00 - 15:29 -> 13
    timeFees.add(new TimeFee("15:30", "16:59", 18)); // 15:30 - 16:59 -> 18
    timeFees.add(new TimeFee("17:00", "17:59", 13)); // 17:00 - 17:59 -> 13
    timeFees.add(new TimeFee("18:00", "18:29", 8)); // 18:00 - 18:29 -> 8

    // Change date to local time
    LocalTime localTimeDate = dateToLocaleTime(date);

    // Return the cost of the first match
    for (TimeFee timeFee : timeFees) {
      if (timeFee.isMatch(localTimeDate)) {
        return timeFee.getCost();
      }
    }

    // If no match was found, return default (0)
    return 0;
  }

  private LocalTime dateToLocaleTime(Date date) {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);
    int hour = calendar.get(Calendar.HOUR_OF_DAY);
    int minute = calendar.get(Calendar.MINUTE);

    return LocalTime.of(hour, minute);
  }

  private boolean isHoliday(Calendar calendar){
    HashMap<Integer, Integer[]> holidaysPerMonth = new HashMap<>();

    holidaysPerMonth.put(Calendar.JANUARY, new Integer[] { 1 });
    holidaysPerMonth.put(Calendar.MARCH, new Integer[] { 28, 29 });
    holidaysPerMonth.put(Calendar.APRIL, new Integer[] { 1, 30 });
    holidaysPerMonth.put(Calendar.MAY, new Integer[] { 1, 8, 9 });
    holidaysPerMonth.put(Calendar.JUNE, new Integer[] { 5, 6, 21 });
    holidaysPerMonth.put(Calendar.JULY, new Integer[] {  });
    holidaysPerMonth.put(Calendar.NOVEMBER, new Integer[] { 1 });
    holidaysPerMonth.put(Calendar.DECEMBER, new Integer[] { 24, 25, 26, 31 });

    for(Integer holiday : holidaysPerMonth.get(calendar.get(Calendar.MONTH))){
      if(holiday.equals(calendar.get(Calendar.DAY_OF_MONTH))){
        return true;
      }
    }

    return false;
  }

  private boolean isWeekend(Calendar calendar){
    int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
    return (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY);
  }

  private Boolean isTollFreeDate(Date date) {
    Calendar calendar = GregorianCalendar.getInstance();
    calendar.setTime(date);

    return isWeekend(calendar) || isHoliday(calendar);
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

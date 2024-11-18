package com.afry.tollcalculator;

import java.time.LocalDateTime;
import java.time.ZoneId;
import java.util.*;
import java.util.function.Predicate;

public class TollCalculator {

  /**
   * Calculate the total toll fee for one day
   *
   * @param vehicle - the vehicle
   * @param dates   - date and time of all passes on one day
   * @return - the total toll fee for that day
   */
  public int getTotalTollFee(Vehicle vehicle, Date... dates) {

    if (isTollFreeVehicle(vehicle)) {
      return 0;
    }

    // For the purpose of this evaluation:
    // Assume that error handling and sanity check on dates is done by caller

    List<LocalDateTime> localDateTimeList = Arrays.stream(dates).sequential()
            .sorted()
            .filter(Predicate.not(TollCalculator::isTollFreeDate))
            .map(date -> LocalDateTime.ofInstant(date.toInstant(), ZoneId.systemDefault()))
            .toList();

    Map<TollTimeInterval, List<LocalDateTime>> mapOfPassagesByHourInterval = new HashMap<>();

    for (LocalDateTime ldt : localDateTimeList) {
      final int hour = ldt.getHour();
      final int minute = ldt.getMinute();
      final int endHour = (minute == 0) ? hour : hour + 1;
      final int endMinute = (minute == 0) ? 59 : minute - 1;
      TollTimeInterval tti = new TollTimeInterval(hour, minute, endHour, endMinute);

      Optional<TollTimeInterval> match = mapOfPassagesByHourInterval.keySet().stream()
              .filter(t -> t.isInside(ldt.getHour(), ldt.getMinute())).findFirst();

      match.ifPresentOrElse(interval -> mapOfPassagesByHourInterval.get(interval).add(ldt),
                            () -> mapOfPassagesByHourInterval.put(tti, new ArrayList<>(List.of(ldt))));
    }

    Map<TollTimeInterval, Integer> maxFeeByInterval = new HashMap<>(mapOfPassagesByHourInterval.size());

    mapOfPassagesByHourInterval.forEach((tollTimeInterval, passages) -> {
        Optional<Integer> maxFee = passages.stream()
                .map(p -> tollTimes.stream()
                        .filter(t -> t.isInside(p.getHour(), p.getMinute()))
                        .findFirst())
                .filter(Optional::isPresent)
                .map(Optional::get)
                .map(FEEBYINTERVAL::get)
                .reduce(Integer::max);

        maxFeeByInterval.put(tollTimeInterval, maxFee.orElse(0));
    });

    int sumFee = maxFeeByInterval.values().stream().mapToInt(Integer::valueOf).sum();

    return Math.min(60, sumFee);
  }

  static boolean isTollFreeVehicle(Vehicle vehicle) {
    if (vehicle == null) {
      return false;
    }

    String vehicleType = vehicle.getType();
    Optional<TollFreeVehicles> tollFreeVehicle = TollFreeVehicles.fromString(vehicleType);

    if (tollFreeVehicle.isPresent()) {
      return tollFreeVehicle.map(TollFreeVehicles::isTollFree).orElse(false);
    } else {
      // For the purpose of this evaluation:
      // Logging unexpected vehicle type or some agreed upon error handling would have been added here in
      // production code

      return false;
    }

  }

  static boolean isTollFreeDate(Date date) {
    return HolidayUtils.isWeekend(date)
            || HolidayUtils.isInJuly(date)
            || HolidayUtils.isDayBeforeHoliday(date)
            || HolidayUtils.isSwedishFixedHoliday(date)
            || HolidayUtils.isMidsummerEve(date)
            || HolidayUtils.isEastern(date)
            || HolidayUtils.isAscensionDay(date);
  }

  // https://www.transportstyrelsen.se/sv/vagtrafik/trangselskatt/trangselskatt-i-goteborg/Tider-och-belopp-i-Goteborg/
  static final TollTimeInterval tti00000559 = new TollTimeInterval(0, 0, 5, 59);
  static final TollTimeInterval tti06000629 = new TollTimeInterval(6, 0, 6, 29);
  static final TollTimeInterval tti06300659 = new TollTimeInterval(6, 30, 6, 59);
  static final TollTimeInterval tti07000759 = new TollTimeInterval(7, 0, 7, 59);
  static final TollTimeInterval tti08000829 = new TollTimeInterval(8, 0, 8, 29);
  static final TollTimeInterval tti08301459 = new TollTimeInterval(8, 30, 14, 59);
  static final TollTimeInterval tti15001529 = new TollTimeInterval(15, 0, 15, 29);
  static final TollTimeInterval tti15301659 = new TollTimeInterval(15, 30, 16, 59);
  static final TollTimeInterval tti17001759 = new TollTimeInterval(17, 0, 17, 59);
  static final TollTimeInterval tti18001829 = new TollTimeInterval(18, 0, 18, 29);
  static final TollTimeInterval tti18302359 = new TollTimeInterval(18, 30, 23, 59);

  private static final Set<TollTimeInterval> tollTimes = Set.of(tti00000559, tti06000629, tti06300659, tti07000759, tti08000829,
  tti08301459, tti15001529, tti15301659, tti17001759, tti18001829, tti18302359);

  static final Map<TollTimeInterval, Integer> FEEBYINTERVAL = Map.ofEntries(
          Map.entry(tti00000559, 0),
          Map.entry(tti06000629, 9),
          Map.entry(tti06300659, 16),
          Map.entry(tti07000759, 22),
          Map.entry(tti08000829, 16),
          Map.entry(tti08301459, 9),
          Map.entry(tti15001529, 16),
          Map.entry(tti15301659, 22),
          Map.entry(tti17001759, 16),
          Map.entry(tti18001829, 9),
          Map.entry(tti18302359, 0)
  );

  /**
   * Calculate the toll fee for one date
   *
   * @param date - the date/time of a passage
   * @param vehicle - the vehicle
   * @return - the toll fee for that date/time
   */
  public int getTollFee(final Date date, Vehicle vehicle) {
    // For the purpose of this evaluation, assume that this method is part of
    // an agreed upon API. (else I would have changed the order of the arguments to be the same in both methods)
    // Otherwise not used in solution

    if(isTollFreeDate(date) || isTollFreeVehicle(vehicle)) {
      return 0;
    }

    LocalDateTime localDateTime = LocalDateTime.ofInstant(date.toInstant(), ZoneId.systemDefault());
    final int hour = localDateTime.getHour();
    final int minute = localDateTime.getMinute();

    Optional<TollTimeInterval> tollTimeInterval = tollTimes.stream()
            .filter(interval -> interval.isInside(hour, minute))
            .findFirst();

      return tollTimeInterval.map(FEEBYINTERVAL::get).orElse(0);
  }

}


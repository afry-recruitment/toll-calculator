package com.afry.test.tollcalculator.service;

import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalTime;
import java.time.temporal.ChronoUnit;
import java.util.Arrays;
import java.util.Date;

import org.springframework.stereotype.Service;

import com.afry.test.tollcalculator.config.TollConfig;
import com.afry.test.tollcalculator.domain.Vehicle;
import com.afry.test.tollcalculator.spi.TollFeeEvaluator;
import com.afry.test.tollcalculator.spi.TollFreeDateChecker;
import com.afry.test.tollcalculator.spi.TollFreeVehicleChecker;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;

/**
 * Coordinates toll fee calculation for vehicle passes.
 */

@Slf4j
@Service
@RequiredArgsConstructor
public class TollCalculator {

  private final TollConfig tollConfig;
  private final TollFeeEvaluator tollFeeEvaluator;
  private final TollFreeDateChecker tollFreeDateChecker;
  private final TollFreeVehicleChecker tollFreeVehicleChecker;

  /**
   * @param vehicle the vehicle making the pass
   * @param dates   all the dates of the passes of the vehicle
   * @return final toll fee for the vehicle for the day
   */
  public int getTollFee(Vehicle vehicle, Date... dates) {

    if (dates == null || dates.length == 0) {
      log.debug("No dates provided. Returning fee=0");
      return 0;
    }

    // All dates are guaranteed to be on the same day
    LocalDate passDay = dates[0].toInstant().atZone(tollConfig.zoneId()).toLocalDate();
    if (tollFreeDateChecker.isTollFreeDate(passDay) || tollFreeVehicleChecker.isTollFreeVehicle(vehicle)) {
      return 0;
    }

    int tollSumForTheDay = 0;
    int currentIntervalToll = 0;

    Arrays.sort(dates);
    Instant intervalEndInstant = dates[0].toInstant().plus(1, ChronoUnit.HOURS);

    for (Date date : dates) {

      LocalTime passTime = date.toInstant().atZone(tollConfig.zoneId()).toLocalTime();
      final int currentPassToll = tollFeeEvaluator.evaluateFee(passTime);
      final Instant currentPass = date.toInstant();
      if (currentPass.isBefore(intervalEndInstant)) {
        currentIntervalToll = Math.max(currentIntervalToll, currentPassToll);
        continue;
      }
      tollSumForTheDay += currentIntervalToll;
      intervalEndInstant = currentPass.plus(1, ChronoUnit.HOURS);
      currentIntervalToll = currentPassToll;

    }
    tollSumForTheDay += currentIntervalToll;
    int finalFee = Math.min(tollConfig.dailyMaxFee(), tollSumForTheDay);
    log.info("Final toll fee calculated for vehicle={} on {} is {}",
        vehicle.getType(),
        passDay,
        finalFee);

    return finalFee;
  }

}

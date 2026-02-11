package com.afry.test.tollcalculator.spi;

import com.afry.test.tollcalculator.domain.Vehicle;

/**
 * Determines whether a vehicle is exempt from toll fees.
 */
public interface TollFreeVehicleChecker {

  /**
   * Checks whether the given vehicle is toll-free.
   *
   * @param vehicle the vehicle to check
   * @return {@code true} if the vehicle is exempt from toll fees
   */
  boolean isTollFreeVehicle(Vehicle vehicle);
}

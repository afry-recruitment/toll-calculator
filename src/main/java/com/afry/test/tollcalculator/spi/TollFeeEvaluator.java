package com.afry.test.tollcalculator.spi;

import java.time.LocalTime;

/**
 * Evaluates the toll fee for a single vehicle pass based on the time of day.
 *
 */
public interface TollFeeEvaluator {

  /**
   * Evaluates the toll fee for a given pass time.
   *
   * @param passTime the time of day when the vehicle passed the toll point
   * @return the applicable toll fee, or {@code 0} if no rule applies
   */
  int evaluateFee(LocalTime passTime);
}

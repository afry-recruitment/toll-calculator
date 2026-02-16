package com.afry.test.tollcalculator.spi;

import java.time.LocalDate;

/**
 * Determines whether a specific date is exempt from toll fees.
 */
public interface TollFreeDateChecker {

  /**
   * Checks whether the given date is toll-free.
   *
   * @param date the date to evaluate
   * @return {@code true} if the date is exempt from toll fees
   */
  boolean isTollFreeDate(LocalDate date);
}

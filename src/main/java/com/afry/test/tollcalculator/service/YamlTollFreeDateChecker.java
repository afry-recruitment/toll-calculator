package com.afry.test.tollcalculator.service;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.util.Objects;

import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Component;

import com.afry.test.tollcalculator.config.TollConfig;
import com.afry.test.tollcalculator.spi.TollFreeDateChecker;

import lombok.RequiredArgsConstructor;

/**
 * YAML-backed implementation of {@link TollFreeDateChecker}.
 *
 * <p>This checker determines whether a given date is exempt from toll fees
 * based on weekend rules and a list of configured toll-free dates loaded
 * from application YAML configuration.</p>
 */
@Component
@RequiredArgsConstructor
@Profile("!db")
public class YamlTollFreeDateChecker implements TollFreeDateChecker {

  private final TollConfig tollConfig;

  @Override
  public boolean isTollFreeDate(LocalDate date) {
    Objects.requireNonNull(date, "date must not be null");
    return date.getDayOfWeek() == DayOfWeek.SATURDAY ||
        date.getDayOfWeek() == DayOfWeek.SUNDAY ||
        tollConfig.tollFreeDates().contains(date);
  }
}

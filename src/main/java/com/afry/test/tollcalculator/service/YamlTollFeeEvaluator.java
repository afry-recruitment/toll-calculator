package com.afry.test.tollcalculator.service;

import java.time.LocalTime;
import java.util.Objects;

import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Component;

import com.afry.test.tollcalculator.config.TollConfig;
import com.afry.test.tollcalculator.domain.TollFeeRule;
import com.afry.test.tollcalculator.spi.TollFeeEvaluator;

import lombok.Data;

/**
 * YAML-backed implementation of {@link TollFeeEvaluator}.
 *
 * <p>This evaluator applies toll fee rules loaded from application YAML
 * configuration and determines the applicable fee based on the time of day.</p>
 */
@Data
@Component
@Profile("!db")
public class YamlTollFeeEvaluator implements TollFeeEvaluator {

  private final TollConfig tollConfig;

  @Override public int evaluateFee(LocalTime passTime) {

    Objects.requireNonNull(passTime, "passTime must not be null");
    return tollConfig.feeRules().stream()
        .filter(rule ->
            !passTime.isBefore(rule.from()) &&
                !passTime.isAfter(rule.to()))
        .mapToInt(TollFeeRule::fee)
        .max()
        .orElse(0);
  }
}

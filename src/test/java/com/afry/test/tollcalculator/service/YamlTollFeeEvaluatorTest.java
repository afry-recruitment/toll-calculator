package com.afry.test.tollcalculator.service;

import static org.assertj.core.api.AssertionsForClassTypes.assertThat;
import static org.mockito.Mockito.when;

import java.time.LocalTime;
import java.util.List;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import com.afry.test.tollcalculator.config.TollConfig;
import com.afry.test.tollcalculator.domain.TollFeeRule;

@ExtendWith(MockitoExtension.class)
class YamlTollFeeEvaluatorTest {

  @Mock
  private TollConfig tollConfig;

  @InjectMocks
  private YamlTollFeeEvaluator evaluator;

  @Test
  void shouldReturnHighestFeeForMatchingTimeInterval() {
    LocalTime passTime = LocalTime.of(6, 45);

    List<TollFeeRule> rules = List.of(
        new TollFeeRule(LocalTime.of(6, 0), LocalTime.of(6, 29), 8),
        new TollFeeRule(LocalTime.of(6, 30), LocalTime.of(6, 59), 13)
    );

    when(tollConfig.feeRules()).thenReturn(rules);
    int fee = evaluator.evaluateFee(passTime);

    assertThat(fee).isEqualTo(13);
  }

  void shouldReturnZeroWhenNoRuleMatches() {
    LocalTime passTime = LocalTime.of(6, 30);
    List<TollFeeRule> rules = List.of(
        new TollFeeRule(LocalTime.of(6, 0), LocalTime.of(6, 29), 8),
        new TollFeeRule(LocalTime.of(6, 30), LocalTime.of(6, 59), 13)
    );

    when(tollConfig.feeRules()).thenReturn(rules);
    int fee = evaluator.evaluateFee(passTime);

    assertThat(fee).isZero();

  }

  @Test
  void shouldApplyFeeWhenPassTimeIsExactlyOnIntervalBoundary() {
    LocalTime passTime = LocalTime.of(6, 30);
    List<TollFeeRule> rules = List.of(
        new TollFeeRule(LocalTime.of(6, 0), LocalTime.of(6, 29), 8),
        new TollFeeRule(LocalTime.of(6, 30), LocalTime.of(6, 59), 13)
    );

    when(tollConfig.feeRules()).thenReturn(rules);
    int fee = evaluator.evaluateFee(passTime);

    assertThat(fee).isEqualTo(13);

  }

}

package com.afry.test.tollcalculator.service;

import static org.assertj.core.api.AssertionsForClassTypes.assertThat;
import static org.mockito.Mockito.when;

import java.time.LocalDate;
import java.util.List;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import com.afry.test.tollcalculator.config.TollConfig;

@ExtendWith(MockitoExtension.class)
class YamlTollFreeDateCheckerTest {

  @Mock
  private TollConfig tollConfig;

  @InjectMocks
  private YamlTollFreeDateChecker checker;

  @Test
  void shouldReturnTrueForWeekend() {

    LocalDate saturday = LocalDate.of(2026, 2, 7);
    boolean result = checker.isTollFreeDate(saturday);

    assertThat(result).isTrue();
  }

  @Test
  void shouldReturnTrueForConfiguredTollFreeDate() {

    LocalDate holiday = LocalDate.of(2026, 2, 10);
    when(tollConfig.tollFreeDates()).thenReturn(List.of(holiday));

    boolean result = checker.isTollFreeDate(holiday);

    assertThat(result).isTrue();
  }

  @Test
  void shouldReturnFalseForNormalWeekday() {

    LocalDate weekday = LocalDate.of(2026, 2, 10);
    when(tollConfig.tollFreeDates()).thenReturn(List.of());

    boolean result = checker.isTollFreeDate(weekday);

    assertThat(result).isFalse();
  }
}

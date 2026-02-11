package com.afry.test.tollcalculator.service;

import static org.assertj.core.api.AssertionsForClassTypes.assertThat;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.when;

import java.time.LocalTime;
import java.time.ZoneId;
import java.util.Date;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import com.afry.test.tollcalculator.config.TollConfig;
import com.afry.test.tollcalculator.domain.Car;
import com.afry.test.tollcalculator.domain.Vehicle;
import com.afry.test.tollcalculator.spi.TollFeeEvaluator;
import com.afry.test.tollcalculator.spi.TollFreeDateChecker;
import com.afry.test.tollcalculator.spi.TollFreeVehicleChecker;

@ExtendWith(MockitoExtension.class)
class TollCalculatorTest {

  private static final ZoneId ZONE = ZoneId.of("Europe/Stockholm");

  @Mock
  private TollConfig tollConfig;

  @Mock
  private TollFeeEvaluator tollFeeEvaluator;

  @Mock
  private TollFreeDateChecker tollFreeDateChecker;

  @Mock
  private TollFreeVehicleChecker tollFreeVehicleChecker;

  @InjectMocks
  private TollCalculator tollCalculator;

  @Test
  void shouldReturnZeroForNoDates() {

    Vehicle car = new Car();
    int fee = tollCalculator.getTollFee(car);

    assertThat(fee).isZero();
  }

  @Test
  void shouldReturnZeroIfVehicleOrDateIsTollFree() {

    Vehicle car = new Car();
    Date passDate = new Date();

    when(tollFreeDateChecker.isTollFreeDate(any())).thenReturn(true);
    when(tollConfig.zoneId()).thenReturn(ZONE);
    int fee1 = tollCalculator.getTollFee(car, passDate);
    assertThat(fee1).isZero();

    when(tollFreeDateChecker.isTollFreeDate(any())).thenReturn(false);
    when(tollFreeVehicleChecker.isTollFreeVehicle(any())).thenReturn(true);
    int fee2 = tollCalculator.getTollFee(car, passDate);
    assertThat(fee2).isZero();
  }

  @Test
  void shouldCalculateTollCorrectlyForSinglePass() {

    Vehicle car = new Car();
    Date date = getDate(6, 15);

    when(tollConfig.zoneId()).thenReturn(ZONE);
    when(tollFreeDateChecker.isTollFreeDate(any())).thenReturn(false);
    when(tollFreeVehicleChecker.isTollFreeVehicle(any())).thenReturn(false);
    when(tollFeeEvaluator.evaluateFee(any())).thenReturn(8);
    when(tollConfig.dailyMaxFee()).thenReturn(60);

    int fee = tollCalculator.getTollFee(car, date);
    assertThat(fee).isEqualTo(8);
  }

  @Test
  void shouldApplyHourlyRuleAndMaxFee() {

    Vehicle car = new Car();

    Date date1 = getDate(6, 15);
    Date date2 = getDate(6, 45);

    when(tollConfig.zoneId()).thenReturn(ZONE);
    when(tollFreeDateChecker.isTollFreeDate(any())).thenReturn(false);
    when(tollFreeVehicleChecker.isTollFreeVehicle(any())).thenReturn(false);
    when(tollFeeEvaluator.evaluateFee(LocalTime.of(6, 15))).thenReturn(8);
    when(tollFeeEvaluator.evaluateFee(LocalTime.of(6, 45))).thenReturn(13);
    when(tollConfig.dailyMaxFee()).thenReturn(60);

    int fee = tollCalculator.getTollFee(car, date1, date2);

    assertThat(fee).isEqualTo(13);
  }

  @Test
  void shouldNotExceedDailyMaxFee() {

    Vehicle car = new Car();

    Date date1 = getDate(6, 0);
    Date date2 = getDate(7, 0);
    Date date3 = getDate(8, 0);

    when(tollConfig.zoneId()).thenReturn(ZONE);
    when(tollFreeDateChecker.isTollFreeDate(any())).thenReturn(false);
    when(tollFreeVehicleChecker.isTollFreeVehicle(any())).thenReturn(false);
    when(tollFeeEvaluator.evaluateFee(any())).thenReturn(25);
    when(tollConfig.dailyMaxFee()).thenReturn(60);

    int fee = tollCalculator.getTollFee(car, date1, date2, date3);

    assertThat(fee).isEqualTo(60);
  }

  private Date getDate(int hour, int minute) {
    return Date.from(LocalTime.of(hour, minute).atDate(java.time.LocalDate.of(2026, 2, 11))
        .atZone(ZONE).toInstant());
  }
}

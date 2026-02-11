package com.afry.test.tollcalculator.integration;

import static org.assertj.core.api.Assertions.assertThat;

import java.time.LocalDateTime;
import java.util.Date;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.ActiveProfiles;

import com.afry.test.tollcalculator.config.TollConfig;
import com.afry.test.tollcalculator.domain.Car;
import com.afry.test.tollcalculator.domain.Vehicle;
import com.afry.test.tollcalculator.service.TollCalculator;

@SpringBootTest
@ActiveProfiles("test")
class TollFeeCalculationIntegrationTest {

  @Autowired
  private TollCalculator tollCalculator;

  @Autowired
  private TollConfig tollConfig;

  @Test
  void shouldCalculateFeeFromYamlConfiguration() {

    Vehicle car = new Car();

    Date weekDay1 = generateDate(6, 10);
    Date weekDay2 = generateDate(7, 40);

    int fee = tollCalculator.getTollFee(car, weekDay1, weekDay2);

    assertThat(fee).isEqualTo(26);
  }

  private Date generateDate(int hour, int minute) {
    return Date.from(
        LocalDateTime.of(2026, 2, 11, hour, minute)
            .atZone(tollConfig.zoneId())
            .toInstant());
  }

}

package com.afry.test.tollcalculator.service;

import static org.assertj.core.api.AssertionsForClassTypes.assertThat;
import static org.mockito.Mockito.when;

import java.util.List;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import com.afry.test.tollcalculator.config.TollConfig;
import com.afry.test.tollcalculator.domain.Car;
import com.afry.test.tollcalculator.domain.Motorbike;
import com.afry.test.tollcalculator.domain.Vehicle;
import com.afry.test.tollcalculator.domain.VehicleType;

@ExtendWith(MockitoExtension.class)
class YamlTollFreeVehicleCheckerTest {

  @Mock
  private TollConfig tollConfig;

  @InjectMocks
  private YamlTollFreeVehicleChecker checker;

  @Test
  void shouldReturnTrueForTollFreeVehicle() {

    Vehicle vehicle = new Motorbike();
    when(tollConfig.tollFreeVehicles())
        .thenReturn(List.of(VehicleType.MOTORBIKE, VehicleType.TRACTOR));

    boolean result = checker.isTollFreeVehicle(vehicle);

    assertThat(result).isTrue();
  }

  @Test
  void shouldReturnFalseForNonTollFreeVehicle() {

    Vehicle vehicle = new Car();
    when(tollConfig.tollFreeVehicles())
        .thenReturn(List.of(VehicleType.MOTORBIKE));

    boolean result = checker.isTollFreeVehicle(vehicle);

    assertThat(result).isFalse();
  }
}

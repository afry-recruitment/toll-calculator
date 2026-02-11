package com.afry.test.tollcalculator.service;

import java.util.Objects;

import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Component;

import com.afry.test.tollcalculator.config.TollConfig;
import com.afry.test.tollcalculator.domain.Vehicle;
import com.afry.test.tollcalculator.spi.TollFreeVehicleChecker;

import lombok.RequiredArgsConstructor;

/**
 * YAML-backed implementation of {@link TollFreeVehicleChecker}.
 *
 * <p>This checker determines whether a vehicle is exempt from toll fees
 * based on its type, using configuration loaded from application YAML.</p>
 */
@Component
@RequiredArgsConstructor
@Profile("!db")
public class YamlTollFreeVehicleChecker implements TollFreeVehicleChecker {

  private final TollConfig tollConfig;

  @Override
  public boolean isTollFreeVehicle(Vehicle vehicle) {
    Objects.requireNonNull(vehicle, "Vehicle must not be null");
    return tollConfig.tollFreeVehicles()
        .contains(vehicle.getType());
  }
}

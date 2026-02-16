package com.afry.test.tollcalculator.domain;

public class Car implements Vehicle {

  @Override
  public VehicleType getType() {
    return VehicleType.CAR;
  }

}
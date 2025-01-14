package com.toll.calculator.model.vehicle;

import java.util.Date;
import java.util.List;

public interface Vehicle {

  VehicleType getVehicleType();
  String getRegNumber();
  List<Date> getTollDates();
}

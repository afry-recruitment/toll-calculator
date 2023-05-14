package com.tollcalculator.service;

import com.tollcalculator.boObject.TollCalculatorResponse;
import com.tollcalculator.boObject.VehicleType;
import com.tollcalculator.exceptions.InvalidParameterException;

import java.util.Date;
import java.util.List;

public interface TollCalculatorService {

    void isValidCity(String city) throws InvalidParameterException;

    void isValidVehicle(VehicleType vehicle) throws InvalidParameterException;

    TollCalculatorResponse getTaxAmount(String city, VehicleType vehicle, List<Date> dates);

}

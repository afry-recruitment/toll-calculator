package com.tollcalculator.service;

import com.tollcalculator.boObject.TollCalculatorRequest;
import com.tollcalculator.boObject.TollCalculatorResponse;
import com.tollcalculator.boObject.VehicleType;
import com.tollcalculator.exceptions.InvalidParameterException;

import java.util.Date;
import java.util.List;

/**
 * This interface contains all toll fee calculator services
 */
public interface TollCalculatorService {

    /**
     * This method validate request basis on request input
     * @param tollCalculatorRequest
     * @throws InvalidParameterException
     */
    void validateRequest(TollCalculatorRequest tollCalculatorRequest) throws InvalidParameterException;

    /**
     * This method validate city
     */
    void isValidCity(String city) throws InvalidParameterException;

    /**
     * This method validate vehicle
     */
    void isValidVehicle(VehicleType vehicle) throws InvalidParameterException;

    /**
     * This method calculate toll fee
     * @param city
     * @param vehicle
     * @param List<Date>
     * @return TollCalculatorResponse
     */
    TollCalculatorResponse getTaxAmount(String city, VehicleType vehicle, List<Date> dates);

}

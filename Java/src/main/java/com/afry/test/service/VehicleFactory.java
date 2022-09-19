package com.afry.test.service;

import com.afry.test.model.Car;
import com.afry.test.model.Motorbike;
import com.afry.test.model.Vehicle;
import org.springframework.stereotype.Service;

/**
 *  use to generate Vehicle object Based on Vehicle type
 */
@Service
public class VehicleFactory {
    public Vehicle getVehicle(String vehicleType){
        if(vehicleType == null){
            return null;
        }
        if(vehicleType.equalsIgnoreCase("Car")){
            return new Car();

        } else if(vehicleType.equalsIgnoreCase("Motorbike")){
            return new Motorbike();
        }
        return null;
    }
}

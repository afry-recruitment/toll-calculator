package com.toll.util;

import com.toll.enums.TollFreeVehicles;
import com.toll.model.Vehicle;

public class TollFreeCalculatorUtil {

    /**
     * @param vehicle The vehicle in question
     * @return Returns a boolean value. Returns true if the vehicle is toll free and false otherwise
     */
    public static boolean isTollFreeVehicle(Vehicle vehicle) {
        if (vehicle == null)
            return false;
        String vehicleType = vehicle.getType();
        return vehicleType.equals(TollFreeVehicles.MOTORBIKE.getType())
                || vehicleType.equals(TollFreeVehicles.TRACTOR.getType())
                || vehicleType.equals(TollFreeVehicles.EMERGENCY.getType())
                || vehicleType.equals(TollFreeVehicles.DIPLOMAT.getType())
                || vehicleType.equals(TollFreeVehicles.FOREIGN.getType())
                || vehicleType.equals(TollFreeVehicles.MILITARY.getType());
    }


}

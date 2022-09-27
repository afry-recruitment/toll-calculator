package com.toll.util;

import com.toll.enums.TollFreeVehiclesEnum;
import com.toll.model.Vehicle;

public class TollFreeCalculatorUtil {
    private TollFreeCalculatorUtil() {
    }

    /**
     * @param vehicle The vehicle in question
     * @return Returns a boolean value. Returns true if the vehicle is toll free and false otherwise
     */
    public static boolean isTollFreeVehicle(Vehicle vehicle) {
        if (vehicle == null)
            return false;
        String vehicleType = vehicle.getType();
        return vehicleType.equals(TollFreeVehiclesEnum.MOTORBIKE.getType())
                || vehicleType.equals(TollFreeVehiclesEnum.TRACTOR.getType())
                || vehicleType.equals(TollFreeVehiclesEnum.EMERGENCY.getType())
                || vehicleType.equals(TollFreeVehiclesEnum.DIPLOMAT.getType())
                || vehicleType.equals(TollFreeVehiclesEnum.FOREIGN.getType())
                || vehicleType.equals(TollFreeVehiclesEnum.MILITARY.getType());
    }


}

package com.assignment.toll_calculator.utils;

import com.assignment.toll_calculator.models.VehicleType;
import lombok.AccessLevel;
import lombok.NoArgsConstructor;

/**
 * This class contains all the vehicle type related functions supporting the solution.
 */
@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class VehicleTypeUtils {

    public static VehicleType parse(String type) {
        for (VehicleType vehicleType : VehicleType.values()) {
            if (vehicleType.getType().equalsIgnoreCase(type)) {
                return vehicleType;
            }
        }

        throw new IllegalArgumentException(String.format("Cannot parse vehicle type based on %s", type));
    }

}

package com.assignment.toll_calculator.utils;

import com.assignment.toll_calculator.errors.ApiException;
import com.assignment.toll_calculator.models.VehicleType;
import lombok.AccessLevel;
import lombok.NoArgsConstructor;
import org.springframework.http.HttpStatus;

/**
 * This class contains all the vehicle type related functions.
 */
@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class VehicleTypeUtils {

    /**
     * Parse the vehicle type from source
     * @param type - the source of the parsing
     * @return - the parsed vehicle type
     */
    public static VehicleType parse(String type) {
        for (VehicleType vehicleType : VehicleType.values()) {
            if (vehicleType.getType().equalsIgnoreCase(type)) {
                return vehicleType;
            }
        }

        throw new ApiException(String.format("Cannot parse vehicle type based on %s", type), HttpStatus.BAD_REQUEST);
    }

}

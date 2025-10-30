package com.assignment.toll_calculator.models;

import lombok.AllArgsConstructor;
import lombok.Getter;

/**
 * This is the enum for the different vehicle types.
 */
@AllArgsConstructor
@Getter
public enum VehicleType {

    CAR("Car", false),
    MOTORBIKE("Motorbike"),
    TRACTOR("Tractor"),
    EMERGENCY("Emergency"),
    DIPLOMAT("Diplomat"),
    FOREIGN("Foreign"),
    MILITARY("Military");

    private final String type;
    private final boolean tollFree;

    VehicleType(String type) {
        this(type, true);
    }

    public static VehicleType parse(String type) {
        for (VehicleType vehicleType : values()) {
            if (vehicleType.getType().equalsIgnoreCase(type)) {
                return vehicleType;
            }
        }

        throw new IllegalArgumentException(String.format("Cannot parse vehicle type based on %s", type));
    }

}
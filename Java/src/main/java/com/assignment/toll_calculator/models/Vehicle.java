package com.assignment.toll_calculator.models;

import lombok.AllArgsConstructor;
import lombok.Getter;

/**
 * This is the enum for the different vehicle types.
 */
@AllArgsConstructor
@Getter
public enum Vehicle {

    CAR("Car", false),
    MOTORBIKE("Motorbike"),
    TRACTOR("Tractor"),
    EMERGENCY("Emergency"),
    DIPLOMAT("Diplomat"),
    FOREIGN("Foreign"),
    MILITARY("Military");

    private final String type;
    private final boolean tollFree;

    Vehicle(String type) {
        this(type, true);
    }

    public static Vehicle parse(String type) {
        for (Vehicle vehicle: values()) {
            if (vehicle.getType().equalsIgnoreCase(type)) {
                return vehicle;
            }
        }

        throw new IllegalArgumentException(String.format("Cannot match vehicle with type %s", type));
    }

}
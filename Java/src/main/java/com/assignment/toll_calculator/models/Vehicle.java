package com.assignment.toll_calculator.models;

import lombok.AllArgsConstructor;
import lombok.Getter;

/**
 * This is the enum for the different vehicle types.
 */
@AllArgsConstructor
public enum Vehicle {

    CAR("Car", false),
    MOTORBIKE("Motorbike"),
    TRACTOR("Tractor"),
    EMERGENCY("Emergency"),
    DIPLOMAT("Diplomat"),
    FOREIGN("Foreign"),
    MILITARY("Military");

    private final String type;

    @Getter
    private final boolean tollFree;

    Vehicle(String type) {
        this(type, true);
    }

}
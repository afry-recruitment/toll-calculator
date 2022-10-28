package com.afry.model;

public class Car implements Vehicle {
    @Override
    public String getType() {
        return "Car";
    }

    @Override
    public boolean isTollFreeVehicle() {
        return false;
    }
}

package com.afry.vehicles;

public class Car implements Vehicle {
    @Override
    public VehicleType getType() {
        return VehicleType.CAR;
    }

    @Override
    public boolean isTollFree() {
        return false;
    }
}
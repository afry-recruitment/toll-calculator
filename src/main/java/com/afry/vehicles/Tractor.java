package com.afry.vehicles;

public class Tractor implements Vehicle {
    @Override
    public VehicleType getType() {
        return VehicleType.TRACTOR;
    }

    @Override
    public boolean isTollFree() {
        return true;
    }
}
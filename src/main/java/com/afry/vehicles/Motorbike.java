package com.afry.vehicles;

public class Motorbike implements Vehicle {
    @Override
    public VehicleType getType() {
        return VehicleType.MOTORBIKE;
    }

    @Override
    public boolean isTollFree() {
        return true;
    }
}

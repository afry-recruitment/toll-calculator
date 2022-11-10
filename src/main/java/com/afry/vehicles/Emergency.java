package com.afry.vehicles;

public class Emergency implements Vehicle {
    @Override
    public VehicleType getType() {
        return VehicleType.EMERGENCY;
    }

    @Override
    public boolean isTollFree() {
        return true;
    }
}

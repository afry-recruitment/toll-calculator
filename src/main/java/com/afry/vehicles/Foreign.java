package com.afry.vehicles;

public class Foreign implements Vehicle {
    @Override
    public VehicleType getType() {
        return VehicleType.FOREIGN;
    }

    @Override
    public boolean isTollFree() {
        return true;
    }
}

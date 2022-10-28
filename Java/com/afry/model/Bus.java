package com.afry.model;

public class Bus implements Vehicle {
    @Override
    public String getType() {
        return "Bus";
    }

    @Override
    public boolean isTollFreeVehicle() {
        return false;
    }
}

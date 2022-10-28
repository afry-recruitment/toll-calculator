package com.afry.model;

public class Emergency implements Vehicle {
    @Override
    public String getType() {
        return "Emergency";
    }

    @Override
    public boolean isTollFreeVehicle() {
        return true;
    }
}

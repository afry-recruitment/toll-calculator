package com.afry.model;

public class Foreign implements Vehicle {
    @Override
    public String getType() {
        return "Foreign";
    }

    @Override
    public boolean isTollFreeVehicle() {
        return true;
    }
}

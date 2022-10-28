package com.afry.model;

public class Military implements Vehicle {
    @Override
    public String getType() {
        return "Military";
    }

    @Override
    public boolean isTollFreeVehicle() {
        return true;
    }
}

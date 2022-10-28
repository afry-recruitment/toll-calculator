package com.afry.model;

public class Diplomat implements Vehicle {
    @Override
    public String getType() {
        return "Diplomat";
    }

    @Override
    public boolean isTollFreeVehicle() {
        return true;
    }
}

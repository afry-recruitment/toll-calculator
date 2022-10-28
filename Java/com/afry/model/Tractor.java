package com.afry.model;

public class Tractor implements Vehicle {
    @Override
    public String getType() {
        return "Tractor";
    }

    @Override
    public boolean isTollFreeVehicle() {
        return true;
    }
}

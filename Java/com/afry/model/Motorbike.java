package com.afry.model;

public class Motorbike implements Vehicle {
    @Override
    public String getType() {
        return "Motorbike";
    }

    @Override
    public boolean isTollFreeVehicle() {
        return true;
    }
}

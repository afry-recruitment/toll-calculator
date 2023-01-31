package com.toll.calculator.vehicle;

public class Truck implements Vehicle {
    private String regNumber;

    public Truck(String regNumber) {
        this.regNumber = regNumber;
    }

    @Override
    public VehicleType getType() {
        return VehicleType.TRUCK;
    }

    @Override
    public String getRegNumber() {
        return regNumber;
    }

    @Override
    public void setRegNumber(String regNumber) {
        this.regNumber = regNumber;
    }
}

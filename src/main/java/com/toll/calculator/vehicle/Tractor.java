package com.toll.calculator.vehicle;

public class Tractor implements Vehicle {
    private String regNumber;

    public Tractor(String regNumber) {
        this.regNumber = regNumber;
    }

    @Override
    public VehicleType getType() {
        return VehicleType.TRACTOR;
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

package com.toll.calculator.vehicle;

public class Military implements Vehicle {
    private String regNumber;

    public Military(String regNumber) {
        this.regNumber = regNumber;
    }

    @Override
    public VehicleType getType() {
        return VehicleType.MILITARY;
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

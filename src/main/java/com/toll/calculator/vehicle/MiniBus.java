package com.toll.calculator.vehicle;

public class MiniBus implements Vehicle {
    private String regNumber;

    public MiniBus(String regNumber) {
        this.regNumber = regNumber;
    }

    @Override
    public VehicleType getType() {
        return VehicleType.MINIBUS;
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

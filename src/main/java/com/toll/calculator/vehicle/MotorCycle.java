package com.toll.calculator.vehicle;

public class MotorCycle implements Vehicle {
    private String regNumber;

    public MotorCycle(String regNumber) {
        this.regNumber = regNumber;
    }

    @Override
    public VehicleType getType() {
        return VehicleType.MOTORCYCLE;
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

package com.toll.calculator.vehicle.car;

// For simplicity, EmergencyCar includes police cars, ambulances and fire engines
public class EmergencyCar implements Car {
    private String regNumber;

    public EmergencyCar(String regNumber) {
        this.regNumber = regNumber;
    }

    @Override
    public VehicleType getType() {
        return VehicleType.EMERGENCY;
    }

    @Override
    public String getRegNumber() {
        return regNumber;
    }

    @Override
    public void setRegNumber(String regNumber) {
        this.regNumber = regNumber;
    }

    @Override
    public boolean isDiplomatCar() {
        return false;
    }
}

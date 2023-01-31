package com.toll.calculator.vehicle.car;

public class PrivateCar implements Car {
    private final boolean diplomatCar;
    private String regNumber;

    public PrivateCar(boolean diplomatCar, String regNumber) {
        this.diplomatCar = diplomatCar;
        this.regNumber = regNumber;
    }

    @Override
    public VehicleType getType() {
        return diplomatCar ? VehicleType.DIPLOMAT_PRIVATE_CAR : VehicleType.PRIVATE_CAR;
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
        return diplomatCar;
    }

}

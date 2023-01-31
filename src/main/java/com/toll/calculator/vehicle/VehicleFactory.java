package com.toll.calculator.vehicle;

import com.toll.calculator.exception.VehicleException;
import com.toll.calculator.vehicle.car.EmergencyCar;
import com.toll.calculator.vehicle.car.PrivateCar;

import static com.toll.calculator.vehicle.Vehicle.VehicleType.DIPLOMAT_PRIVATE_CAR;

public class VehicleFactory {

    public static Vehicle getVehicle(Vehicle.VehicleType vehicleType, String regNumber) throws VehicleException {
        if (vehicleType == null) {
            return null;
        }
        switch (vehicleType) {
            case PRIVATE_CAR:
            case DIPLOMAT_PRIVATE_CAR:
                boolean isDiplomat = vehicleType == DIPLOMAT_PRIVATE_CAR;
                return new PrivateCar(isDiplomat, regNumber);
            case EMERGENCY:
                return new EmergencyCar(regNumber);
            case BUS:
                return new Bus(regNumber);
            case MINIBUS:
                return new MiniBus(regNumber);
            case MOTORCYCLE:
                return new MotorCycle(regNumber);
            case MILITARY:
                return new Military(regNumber);
            case TRACTOR:
                return new Tractor(regNumber);
            case TRUCK:
                return new Truck(regNumber);
            default:
                throw new VehicleException("Vehicle type \"" + vehicleType + "\" lacks implementation!");
        }
    }
}

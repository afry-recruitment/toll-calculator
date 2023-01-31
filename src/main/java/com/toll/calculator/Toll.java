package com.toll.calculator;

import com.toll.calculator.vehicle.Vehicle;
import org.joda.time.DateTime;

public class Toll {
    private final Vehicle vehicle;
    private final DateTime tollDate;
    private final TollCalculator.TollFee tollFee;

    public Toll(Vehicle vehicle, DateTime tollDate, TollCalculator.TollFee tollFee) {
        this.vehicle = vehicle;
        this.tollDate = tollDate;
        this.tollFee = tollFee;
    }

    public Vehicle getVehicle() {
        return vehicle;
    }

    public DateTime getTollDateTime() {
        return tollDate;
    }

    public TollCalculator.TollFee getFee() {
        return tollFee;
    }

    @Override
    public String toString() {
        return "Toll{" +
                "vehicle=" + vehicle.getType() +
                ", tollDate=" + tollDate +
                ", tollFee=" + tollFee + " - " + tollFee.getFee() +
                '}';
    }
}

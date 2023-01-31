package com.toll.calculator;
import com.toll.calculator.exception.DateException;
import com.toll.calculator.exception.VehicleException;
import com.toll.calculator.vehicle.Vehicle;
import com.toll.calculator.vehicle.VehicleFactory;

import java.util.*;

public class TestRunner {
    public static void main(String[] args) {
        DateHandler dateHandler = initDateHandler(args);
        TollCalculator tollCalculator = new TollCalculator(dateHandler);

        // Calculating toll for each vehicle type, where each vehicle is tested 5 times
        // Each test-run can create up to 8 dates
        for (Vehicle.VehicleType vehicleType : Vehicle.VehicleType.values()) {
            System.out.println("Calculating toll for vehicle: " + vehicleType);
            for (int i = 1; i <= 5; i++) {
                System.out.format("Run: %d - Vehicle: %s\n", i, vehicleType);
                String regNumber;
                if (vehicleType == Vehicle.VehicleType.MILITARY) {
                    regNumber = Utils.getRegNumber(true);
                } else {
                    regNumber = Utils.getRegNumber(false);
                }
                Vehicle vehicle = null;
                try {
                    vehicle = VehicleFactory.getVehicle(vehicleType, regNumber);

                    int nrDates =  Utils.randomizeInt(7) + 1;
                    Date[] dates = dateHandler.getRandomizedDates(nrDates);
                    System.out.println("Dates: " + Arrays.toString(dates));
                    int totalToll = tollCalculator.getTollFee(vehicle, dates);
                    System.out.format("Vehicle: %s with reg number: %s had a total fee of: %d\n",
                            vehicle.getType(), vehicle.getRegNumber(), totalToll);
                } catch (VehicleException | DateException e ) {
                    e.printStackTrace();
                    break;
                }
            }
            System.out.println();
        }
    }

    private static DateHandler initDateHandler(String[] args) {
        DateHandler dateHandler = null;
        if (args.length == 0) {
            dateHandler = new DateHandler();
        } else {
            try {
                int year = Integer.parseInt(args[0]);
                dateHandler = new DateHandler(year);
            } catch (NumberFormatException e) {
                System.err.println("Given argument must represent a year");
                System.exit(1);
            }
        }
        return dateHandler;
    }

}
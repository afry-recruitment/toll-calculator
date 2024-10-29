package com.city.tollcalculator;

import com.city.tollcalculator.vehicles.Car;
import com.city.tollcalculator.vehicles.Motorbike;
import com.city.tollcalculator.utils.TimeRange.parseDate;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class Main {
    public static void main(String[] args) {
        try {
            TollCalculator tollCalculator = new TollCalculator();

            Vehicle car = new Car();
            List<Date> passTimes = new ArrayList<>();

            passTimes.add(parseDate("2023-10-30 06:15")); // 8 SEK
            passTimes.add(parseDate("2023-10-30 07:15")); // 18 SEK
            passTimes.add(parseDate("2023-10-30 08:45")); // 8 SEK
            passTimes.add(parseDate("2023-10-30 15:15")); // 13 SEK
            passTimes.add(parseDate("2023-10-30 16:30")); // 18 SEK
            passTimes.add(parseDate("2023-10-30 18:00")); // 8 SEK

            int totalFee = tollCalculator.getTollFee(passTimes.toArray(new Date[0]), car);
            System.out.println("Total toll fee for the car: " + totalFee + " SEK");

            Vehicle motorbike = new Motorbike();
            totalFee = tollCalculator.getTollFee(motorbike, passTimes.toArray(new Date[0]));
            System.out.println("Total toll fee for the motorbike: " + totalFee + " SEK (should be 0 for toll-free vehicles)");

        } catch (ParseException e) {
            System.out.println("Error parsing date: " + e.getMessage());
        }
    }
}

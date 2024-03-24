package com.codetest;

import com.codetest.calculator.TollCalculator;
import com.codetest.model.Car;
import com.codetest.model.Motorbike;
import org.junit.Test;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

import static org.junit.Assert.assertEquals;

public class TollCalculatorTest {

    private static final TollCalculator tollCalculator = new TollCalculator();
    private static final SimpleDateFormat dateFormat = new SimpleDateFormat ("yyyy-MM-dd HH:mm:ss");

    @Test
    public void testOnePassage() throws ParseException {
        Car car = new Car();
        Date passage = dateFormat.parse("2024-03-25 18:00:00");

        int total = tollCalculator.getTollFee(passage, car);
        assertEquals(8, total);

        total = tollCalculator.getTotalTollFee(car, passage);
        assertEquals(8, total);
    }

    @Test
    public void testMultiplePassageNotWithinHour() throws ParseException {
        Car car = new Car();
        Date[] passages = {
                dateFormat.parse("2024-03-25 05:00:00"), // 0
                dateFormat.parse("2024-03-25 08:00:00"), // 13
                dateFormat.parse("2024-03-25 11:30:00"), // 8
                dateFormat.parse("2024-03-25 13:00:00"), // 8
                dateFormat.parse("2024-03-25 15:45:00"), // 18
                dateFormat.parse("2024-03-25 17:30:00")  // 13
        };

        int total = tollCalculator.getTotalTollFee(car, passages);
        assertEquals(60, total);
    }

    @Test
    public void testMultiplePassageNotWithinHourIncludingHoliday() throws ParseException {
        Car car = new Car();
        Date[] passages = {
                dateFormat.parse("2024-03-29 08:00:00"), // 0
                dateFormat.parse("2024-03-29 11:30:00"), // 0
        };

        int total = tollCalculator.getTotalTollFee(car, passages);
        assertEquals(0, total);
    }

    @Test
    public void testMultiplePassageWithinHour() throws ParseException {
        Car car = new Car();
        Date[] passages = {
                dateFormat.parse("2024-03-25 08:00:00"), // 13
                dateFormat.parse("2024-03-25 11:30:00"), // 8
                dateFormat.parse("2024-03-25 11:59:00"), // 0
                dateFormat.parse("2024-03-25 17:30:00")  // 13
        };

        int total = tollCalculator.getTotalTollFee(car, passages);
        assertEquals(34, total);
    }

    @Test
    public void testTollFreeVehicle() throws ParseException {
        Motorbike motorbike = new Motorbike();
        Date passage = dateFormat.parse("2024-03-25 18:00:00");

        int total = tollCalculator.getTollFee(passage, motorbike);
        assertEquals(0, total);

        total = tollCalculator.getTotalTollFee(motorbike, passage);
        assertEquals(0, total);
    }
}

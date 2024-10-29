package com.city.tollcalculator;

import com.city.tollcalculator.vehicles.Car;
import com.city.tollcalculator.vehicles.Motorbike;
import org.junit.Before;
import org.junit.Test;
import java.util.Calendar;
import java.util.Date;

import static org.junit.Assert.assertEquals;

public class TollCalculatorTest {
    private TollCalculator tollCalculator;
    private Vehicle car;
    private Vehicle motorbike;

    @Before
    public void setUp() {
        tollCalculator = new TollCalculator();
        car = new Car();
        motorbike = new Motorbike();
    }

    private Date createDate(int hour, int minute) {
        Calendar calendar = Calendar.getInstance();
        calendar.set(Calendar.HOUR_OF_DAY, hour);
        calendar.set(Calendar.MINUTE, minute);
        calendar.set(Calendar.SECOND, 0);
        calendar.set(Calendar.MILLISECOND, 0);
        return calendar.getTime();
    }

    @Test
    public void testFeeAtDifferentTimes() {
        assertEquals(8, tollCalculator.getTollFee(car, createDate(6, 15)));
        assertEquals(13, tollCalculator.getTollFee(car, createDate(6, 45)));
        assertEquals(18, tollCalculator.getTollFee(car, createDate(7, 30)));
        assertEquals(13, tollCalculator.getTollFee(car, createDate(8, 15)));
        assertEquals(8, tollCalculator.getTollFee(car, createDate(15, 45)));
        assertEquals(18, tollCalculator.getTollFee(car, createDate(16, 30)));
        assertEquals(13, tollCalculator.getTollFee(car, createDate(17, 45)));
        assertEquals(8, tollCalculator.getTollFee(car, createDate(18, 15)));
    }

    @Test
    public void testMaximumDailyFeeCap() {
        Date[] dates = {
            createDate(6, 0), createDate(7, 0), createDate(8, 0),
            createDate(9, 0), createDate(10, 0), createDate(11, 0),
            createDate(12, 0), createDate(13, 0), createDate(14, 0),
            createDate(15, 0), createDate(16, 0), createDate(17, 0),
            createDate(18, 0)
        };
        assertEquals(60, tollCalculator.getTollFee(car, dates));
    }

    @Test
    public void testTollFreeVehicle() {
        Date[] dates = { createDate(6, 0), createDate(7, 0), createDate(8, 0) };
        assertEquals(0, tollCalculator.getTollFee(motorbike, dates));
    }

    @Test
    public void testWeekendTollFree() {
        Calendar calendar = Calendar.getInstance();

        calendar.set(Calendar.DAY_OF_WEEK, Calendar.SATURDAY);
        Date saturday = calendar.getTime();

        calendar.set(Calendar.DAY_OF_WEEK, Calendar.SUNDAY);
        Date sunday = calendar.getTime();

        assertEquals(0, tollCalculator.getTollFee(car, saturday, sunday));
    }
}

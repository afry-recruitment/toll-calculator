package com.afry.test;

import com.afry.model.Bus;
import com.afry.model.Military;
import com.afry.model.Vehicle;
import com.afry.service.TollCalculatorService;
import org.junit.Test;

import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.Month;

import static org.junit.Assert.*;

public class TollCalculatorServiceTest {

    private TollCalculatorService tollCalculatorService = new TollCalculatorService();

    @Test
    public void testIsInRange() {
        LocalDateTime dateTime1 = LocalDateTime.of(2022, Month.OCTOBER, 3, 2, 4, 0);
        assertFalse(tollCalculatorService.isInRange(dateTime1.toLocalTime(), LocalTime.of(7, 0), LocalTime.of(8, 29)));

        LocalDateTime dateTime2 = LocalDateTime.of(2022, Month.OCTOBER, 3, 12, 0, 05);
        assertTrue(tollCalculatorService.isInRange(dateTime2.toLocalTime(), LocalTime.of(12, 0), LocalTime.of(13, 59)));

    }

    @Test
    public void testGetFee() {
        LocalDateTime dateTime1 = LocalDateTime.of(2022, Month.OCTOBER, 14, 2, 4, 0);
        int fee1=tollCalculatorService.getFee(dateTime1);
        assertNotNull(fee1);
        assertEquals(8,fee1);

        LocalDateTime dateTime2 = LocalDateTime.of(2022, Month.OCTOBER, 14, 7, 0, 0);
        assertEquals(18,tollCalculatorService.getFee(dateTime2));

        LocalDateTime dateTime3 = LocalDateTime.of(2022, Month.OCTOBER, 14, 13, 59, 59);
        assertEquals(13,tollCalculatorService.getFee(dateTime3));

        LocalDateTime dateTime4 = LocalDateTime.of(2022, Month.OCTOBER, 14, 14, 0, 0);
        assertEquals(8,tollCalculatorService.getFee(dateTime4));

        LocalDateTime dateTime5 = LocalDateTime.of(2022, Month.OCTOBER, 14, 17, 30, 59);
        assertEquals(13,tollCalculatorService.getFee(dateTime5));
    }

    @Test
    public void testGetTollFreeForTollFreeVehicle() {

        Vehicle military = new Military();
        LocalDateTime[] tollPassedTimings = new LocalDateTime[2];
        tollPassedTimings[0] = LocalDateTime.of(2022, Month.OCTOBER, 14, 17, 30, 59);
        tollPassedTimings[1] = LocalDateTime.of(2022, Month.OCTOBER, 14, 17, 52, 59);
        int feeForMilitary=tollCalculatorService.getTollFee(military, tollPassedTimings);

        assertNotNull(feeForMilitary);
        assertEquals(0,feeForMilitary);

        Vehicle bus= new Bus();
        assertTrue(tollCalculatorService.getTollFee(bus, tollPassedTimings) != 0);
    }

    @Test
    public void testGetTollFreeForTollFreeDay() {

        Vehicle bus = new Bus();
        LocalDateTime[] tollPassedOnSunday = new LocalDateTime[2];
        tollPassedOnSunday[0] = LocalDateTime.of(2022, Month.OCTOBER, 30, 17, 30, 59);
        tollPassedOnSunday[1] = LocalDateTime.of(2022, Month.OCTOBER, 30, 17, 52, 59);
        assertEquals(0,tollCalculatorService.getTollFee(bus, tollPassedOnSunday));

        LocalDateTime[] tollPassedOnChristmas = new LocalDateTime[2];
        tollPassedOnChristmas[0] = LocalDateTime.of(2022, Month.DECEMBER, 25, 17, 30, 59);
        tollPassedOnChristmas[1] = LocalDateTime.of(2022, Month.DECEMBER, 30, 17, 52, 59);

        assertEquals(0,tollCalculatorService.getTollFee(bus, tollPassedOnChristmas));
    }

    @Test
    public void testGetTollFreeFor3PassesWithFeeLessThan60() {

        Vehicle bus = new Bus();
        LocalDateTime[] tollPassedTimings = new LocalDateTime[3];
        //The fee will be 8+13+18=39
        tollPassedTimings[0] = LocalDateTime.of(2022, Month.OCTOBER, 28, 8, 30, 59);
        tollPassedTimings[1] = LocalDateTime.of(2022, Month.OCTOBER, 28, 12, 25, 59);
        tollPassedTimings[2] = LocalDateTime.of(2022, Month.OCTOBER, 28, 16, 25, 59);
        int tollFee=tollCalculatorService.getTollFee(bus, tollPassedTimings);
        assertTrue(tollFee <60);
        assertEquals(39,tollFee);
    }

    @Test
    public void testGetTollFreeFor6PassesWithFeeMoreThan60() {

        Vehicle bus = new Bus();
        LocalDateTime[] tollPassedTimings = new LocalDateTime[6];
        //The fee will be 8+13+18+18+8+8 =73, but as per logic it should return 60
        tollPassedTimings[0] = LocalDateTime.of(2022, Month.OCTOBER, 28, 8, 30, 59);
        tollPassedTimings[1] = LocalDateTime.of(2022, Month.OCTOBER, 28, 12, 25, 59);
        tollPassedTimings[2] = LocalDateTime.of(2022, Month.OCTOBER, 28, 15, 51, 59);
        tollPassedTimings[3] = LocalDateTime.of(2022, Month.OCTOBER, 28, 17, 25, 59);
        tollPassedTimings[4] = LocalDateTime.of(2022, Month.OCTOBER, 28, 19, 55, 59);
        tollPassedTimings[5] = LocalDateTime.of(2022, Month.OCTOBER, 28, 22, 25, 59);
        int tollFee=tollCalculatorService.getTollFee(bus, tollPassedTimings);
        assertEquals(60,tollFee);
    }

    @Test
    public void testGetTollFreeFor3PassesWithinAnHour() {

        Vehicle bus = new Bus();
        LocalDateTime[] tollPassedTimings = new LocalDateTime[3];
        //The fee will be 8+13+18+18+8 =65, but as per logic it should return 60
        tollPassedTimings[0] = LocalDateTime.of(2022, Month.OCTOBER, 28, 6, 45, 59);
        tollPassedTimings[1] = LocalDateTime.of(2022, Month.OCTOBER, 28, 7, 25, 59);
        tollPassedTimings[2] = LocalDateTime.of(2022, Month.OCTOBER, 28, 7, 45, 59);

        int tollFee=tollCalculatorService.getTollFee(bus, tollPassedTimings);
        assertTrue(tollFee<60);
        assertEquals(18,tollFee);
    }

    @Test
    public void testGetTollFreeForADay() {

        Vehicle bus = new Bus();
        LocalDateTime[] tollPassedTimings = new LocalDateTime[6];
        //The fee will be 8+13+18+18+8 =65, but as per logic it should return 60
        tollPassedTimings[0] = LocalDateTime.of(2022, Month.OCTOBER, 28, 6, 45, 59);
        tollPassedTimings[1] = LocalDateTime.of(2022, Month.OCTOBER, 28, 7, 25, 59);
        tollPassedTimings[2] = LocalDateTime.of(2022, Month.OCTOBER, 28, 8, 45, 59);
        tollPassedTimings[3] = LocalDateTime.of(2022, Month.OCTOBER, 28, 11, 45, 59);
        tollPassedTimings[4] = LocalDateTime.of(2022, Month.OCTOBER, 28, 12, 45, 59);
        tollPassedTimings[5] = LocalDateTime.of(2022, Month.OCTOBER, 28, 22, 45, 59);
        int tollFee=tollCalculatorService.getTollFee(bus, tollPassedTimings);
        assertTrue(tollFee<60);
        assertEquals(47,tollFee);
    }
}

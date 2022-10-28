package com.afry.test;

import com.afry.service.TollFreeDayService;
import org.junit.Test;

import java.time.LocalDateTime;
import java.time.Month;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

public class TollFreeDayServiceTest {

    private TollFreeDayService tollFreeDayService=new TollFreeDayService();

    @Test
 public void testIsWeekend(){
        LocalDateTime saturday= LocalDateTime.of(2022,Month.OCTOBER,2,1,2,3);
        assertTrue(tollFreeDayService.isWeekend(saturday));

        LocalDateTime sunday= LocalDateTime.of(2022,10,8,2,4,0);
        assertTrue(tollFreeDayService.isWeekend(sunday));
 }

    @Test
    public void testIsNotWeekend(){
        LocalDateTime weekday= LocalDateTime.of(2022, Month.OCTOBER,27,10,12,0);
        assertFalse(tollFreeDayService.isWeekend(weekday));
    }

    @Test
    public void testIsGenericHoliday() {
        //Considering 2022 generic Holidays
        LocalDateTime newYear = LocalDateTime.of(2022, Month.JANUARY, 1, 10, 12, 0);
        assertTrue(tollFreeDayService.isGenericHoliday(newYear));

        LocalDateTime nationalDay = LocalDateTime.of(2022, Month.JUNE, 6, 1, 2, 3);
        assertTrue(tollFreeDayService.isGenericHoliday(nationalDay));

        LocalDateTime epiphany = LocalDateTime.of(2022, Month.JANUARY, 6, 2, 4, 0);
        assertTrue(tollFreeDayService.isGenericHoliday(epiphany));

        LocalDateTime laboursDay = LocalDateTime.of(2023, Month.MAY, 1, 2, 4, 0);
        assertTrue(tollFreeDayService.isGenericHoliday(laboursDay));

        LocalDateTime christmasEve = LocalDateTime.of(2022, Month.DECEMBER, 24, 2, 4, 0);
        assertTrue(tollFreeDayService.isGenericHoliday(christmasEve));

        LocalDateTime christmasDay = LocalDateTime.of(2022, Month.DECEMBER, 25, 2, 4, 0);
        assertTrue(tollFreeDayService.isGenericHoliday(christmasDay));

        LocalDateTime stStephensDay = LocalDateTime.of(2021, Month.DECEMBER, 26, 2, 4, 0);
        assertTrue(tollFreeDayService.isGenericHoliday(stStephensDay));

        LocalDateTime newYearsEve = LocalDateTime.of(2022, Month.DECEMBER, 31, 2, 4, 0);
        assertTrue(tollFreeDayService.isGenericHoliday(newYearsEve));
    }

    @Test
    public void testIsNotGenericHoliday() {
        LocalDateTime normalCalendarDay = LocalDateTime.of(2022,Month.OCTOBER,30,2,4,0);
        assertFalse(tollFreeDayService.isGenericHoliday(normalCalendarDay));
    }

    @Test
    public void testCalculateNotAnEasterDay(){

        int year2021 = 2021;
        String normalDay ="2021-04-07";
        assertFalse(normalDay.equals(tollFreeDayService.calculateEasterDayForTheYear(year2021)));

        String christmas2023 ="2023-12-25";
        int year2023=2023;
        assertFalse(christmas2023.equals(tollFreeDayService.calculateEasterDayForTheYear(year2023)));

    }

    @Test
    public void testCalculateEasterDayForTheYear(){
        int year2022 = 2022;
        String easter2022 ="2022-04-17";
        assertTrue(easter2022.equals(tollFreeDayService.calculateEasterDayForTheYear(year2022)));

        int year2021 = 2021;
        String easter2021 ="2021-04-04";
        assertTrue(easter2021.equals(tollFreeDayService.calculateEasterDayForTheYear(year2021)));

        String easter2023="2023-04-09";
        int year2023=2023;
        assertTrue(easter2023.equals(tollFreeDayService.calculateEasterDayForTheYear(year2023)));
    }

    @Test
    public void testIsSwedenHoliday() {
        LocalDateTime easterMonday2021 = LocalDateTime.of(2021,Month.APRIL,5,2,4,0);
        assertTrue(tollFreeDayService.isSwedenHoliday(easterMonday2021));

        LocalDateTime ascensionDay2021 = LocalDateTime.of(2021,Month.MAY,13,2,4,0);
        assertTrue(tollFreeDayService.isSwedenHoliday(ascensionDay2021));
    }

    @Test
    public void testIsNotSwedenHoliday() {
        LocalDateTime normalCalendarDay = LocalDateTime.of(2022,Month.OCTOBER,30,2,4,0);
        assertFalse(tollFreeDayService.isSwedenHoliday(normalCalendarDay));
    }

    @Test
    public void testIsTollFreeDay() {
        LocalDateTime easterMonday2021 = LocalDateTime.of(2021,Month.APRIL,5,2,4,0);
        assertTrue(tollFreeDayService.isTollFreeDay(easterMonday2021));

        LocalDateTime sunday = LocalDateTime.of(2022,Month.OCTOBER,30,2,4,0);
        assertTrue(tollFreeDayService.isTollFreeDay(sunday));

        LocalDateTime newYear2023 = LocalDateTime.of(2023,Month.JANUARY,1,2,4,0);
        assertTrue(tollFreeDayService.isTollFreeDay(newYear2023));
    }

    @Test
    public void testIsNotTollFreeDay() {
        LocalDateTime normalCalendarDay = LocalDateTime.of(2022,Month.OCTOBER,31,2,4,0);
        assertFalse(tollFreeDayService.isTollFreeDay(normalCalendarDay));
    }

}

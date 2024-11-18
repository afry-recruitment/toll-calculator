package com.afry.tollcalculator;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.Calendar;
import java.util.Date;
import java.util.stream.Stream;

import static org.junit.jupiter.api.Assertions.*;

class HolidayUtilsTest {

    private static Stream<Arguments> provideDataForIsWeekend() {
        return Stream.of(
                Arguments.of(createDate(2024, Calendar.JANUARY, 12), false),
                Arguments.of(createDate(2024, Calendar.JANUARY, 13), true),
                Arguments.of(createDate(2024, Calendar.JANUARY, 14), true),
                Arguments.of(createDate(2024, Calendar.JANUARY, 15), false)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsWeekend")
    void isWeekend(Date date, boolean expected) {
        assertEquals(expected, HolidayUtils.isWeekend(date));
    }

    private static Stream<Arguments> provideDataForIsInJuly() {
        return Stream.of(
                Arguments.of(createDate(2024, Calendar.AUGUST, 1), false),
                Arguments.of(createDate(2024, Calendar.JULY, 13), true),
                Arguments.of(createDate(2024, Calendar.JULY, 14), true),
                Arguments.of(createDate(2024, Calendar.JUNE, 30), false)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsInJuly")
    void isInJuly(Date date, boolean expected) {
        assertEquals(expected, HolidayUtils.isInJuly(date));
    }

    private static Stream<Arguments> provideDataForIsSwedishFixedHoliday() {
        return Stream.of(
                Arguments.of(createDate(2024, Calendar.JANUARY, 6), true),
                Arguments.of(createDate(2024, Calendar.JANUARY, 5), false),
                Arguments.of(createDate(2024, Calendar.JANUARY, 7), false),
                Arguments.of(createDate(2025, Calendar.JANUARY, 6), true),
                Arguments.of(createDate(2025, Calendar.JANUARY, 5), false),
                Arguments.of(createDate(2025, Calendar.JANUARY, 7), false),
                Arguments.of(createDate(2024, Calendar.JANUARY, 31), false),
                Arguments.of(createDate(2024, Calendar.JUNE, 5), false),
                Arguments.of(createDate(2024, Calendar.DECEMBER, 24), false),
                Arguments.of(createDate(2024, Calendar.JANUARY, 1), true),
                Arguments.of(createDate(2024, Calendar.MAY, 1), true),
                Arguments.of(createDate(2024, Calendar.JUNE, 6), true),
                Arguments.of(createDate(2024, Calendar.DECEMBER, 26), true),
                Arguments.of(createDate(2024, Calendar.DECEMBER, 25), true)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsSwedishFixedHoliday")
    void isSwedishFixedHoliday(Date date, boolean expected) {
        assertEquals(expected, HolidayUtils.isSwedishFixedHoliday(date));
    }

    private static Stream<Arguments> provideDataForIsMidsummerEve() {
        return Stream.of(
                Arguments.of(createDate(2024, Calendar.JUNE, 20), false),
                Arguments.of(createDate(2024, Calendar.JUNE, 21), true),
                Arguments.of(createDate(2024, Calendar.JUNE, 22), false),
                Arguments.of(createDate(2026, Calendar.JUNE, 18), false),
                Arguments.of(createDate(2026, Calendar.JUNE, 19), true),
                Arguments.of(createDate(2026, Calendar.JUNE, 20), false)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsMidsummerEve")
    void isMidsummerEve(Date date, boolean expected) {
        assertEquals(expected, HolidayUtils.isMidsummerEve(date));
    }


    private static Stream<Arguments> provideDataForIsEastern() {
        return Stream.of(
                Arguments.of(createDate(2024, Calendar.MARCH, 29), true),
                Arguments.of(createDate(2024, Calendar.MARCH, 30), true),
                Arguments.of(createDate(2024, Calendar.MARCH, 31), true),
                Arguments.of(createDate(2024, Calendar.APRIL, 1), true),
                Arguments.of(createDate(2025, Calendar.APRIL, 18), true),
                Arguments.of(createDate(2025, Calendar.APRIL, 19), true),
                Arguments.of(createDate(2025, Calendar.APRIL, 20), true),
                Arguments.of(createDate(2025, Calendar.APRIL, 21), true),
                Arguments.of(createDate(2025, Calendar.JUNE, 18), false),
                Arguments.of(createDate(2026, Calendar.JUNE, 19), false),
                Arguments.of(createDate(2026, Calendar.JUNE, 20), false)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsEastern")
    void isEastern(Date date, boolean expected) {
        assertEquals(expected, HolidayUtils.isEastern(date));
    }

    private static Stream<Arguments> provideDataForIsAllSaintsDay() {
        return Stream.of(
                Arguments.of(createDate(2024, Calendar.NOVEMBER, 2), true),
                Arguments.of(createDate(2024, Calendar.NOVEMBER, 1), false),
                Arguments.of(createDate(2024, Calendar.NOVEMBER, 3), false),                Arguments.of(createDate(2027, Calendar.NOVEMBER, 6), true),
                Arguments.of(createDate(2027, Calendar.NOVEMBER, 5), false),
                Arguments.of(createDate(2027, Calendar.NOVEMBER, 7), false),
                Arguments.of(createDate(2026, Calendar.OCTOBER, 31), true),
                Arguments.of(createDate(2026, Calendar.OCTOBER, 30), false),
                Arguments.of(createDate(2026, Calendar.NOVEMBER, 1), false)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsAllSaintsDay")
    void isAllSaintsDay(Date date, boolean expected) {
        assertEquals(expected, HolidayUtils.isAllSaintsDay(date));
    }

    private static Stream<Arguments> provideDataForIsDayBeforeHoliday() {
        return Stream.of(Arguments.of(createDate(2024, Calendar.JANUARY, 1), false),
                Arguments.of(createDate(2023, Calendar.DECEMBER, 31), true),
                Arguments.of(createDate(2024, Calendar.DECEMBER, 24), true),
                Arguments.of(createDate(2024, Calendar.DECEMBER, 25), true),
                Arguments.of(createDate(2024, Calendar.DECEMBER, 23), false),
                Arguments.of(createDate(2024, Calendar.JANUARY, 6), false),
                Arguments.of(createDate(2025, Calendar.JANUARY, 7), false),
                Arguments.of(createDate(2024, Calendar.MARCH, 28), true),
                Arguments.of(createDate(2024, Calendar.MARCH, 29), true),
                Arguments.of(createDate(2024, Calendar.APRIL, 30), true),
                Arguments.of(createDate(2024, Calendar.MAY, 1), false),                Arguments.of(createDate(2024, Calendar.JANUARY, 6), false),
                Arguments.of(createDate(2024, Calendar.JUNE, 5), true),                Arguments.of(createDate(2024, Calendar.JANUARY, 6), false),
                Arguments.of(createDate(2024, Calendar.JUNE, 6), false)
                );
    }

    public static Stream<Arguments> provideDataForIsAscensionDay() {
        return Stream.of(
                Arguments.of(createDate(2023, Calendar.MAY, 18), true),
                Arguments.of(createDate(2023, Calendar.MAY, 17), false),
                Arguments.of(createDate(2023, Calendar.MAY, 19), false),
                Arguments.of(createDate(2024, Calendar.MAY, 9), true),
                Arguments.of(createDate(2024, Calendar.MAY, 8), false),
                Arguments.of(createDate(2024, Calendar.MAY, 10), false),
                Arguments.of(createDate(2025, Calendar.MAY, 29), true),
                Arguments.of(createDate(2025, Calendar.MAY, 28), false),
                Arguments.of(createDate(2025, Calendar.MAY, 30), false),
                Arguments.of(createDate(2026, Calendar.MAY, 14), true)
        );
    }
    @ParameterizedTest
    @MethodSource("provideDataForIsAscensionDay")
    void isAscensionDay(Date date, boolean expected) {
        assertEquals(expected, HolidayUtils.isAscensionDay(date));
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsDayBeforeHoliday")
    void isDayBeforeHoliday(Date date, boolean expected) {
        assertEquals(expected, HolidayUtils.isDayBeforeHoliday(date));
    }

    private static Date createDate(int year, int month, int day) {
        Calendar calendar = Calendar.getInstance();
        calendar.set(year, month, day);

        return calendar.getTime();
    }
}
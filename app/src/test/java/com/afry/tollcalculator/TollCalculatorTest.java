package com.afry.tollcalculator;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.time.*;
import java.util.Calendar;
import java.util.Date;
import java.util.stream.Stream;

import static com.afry.tollcalculator.TollCalculator.FEEBYINTERVAL;
import static org.junit.jupiter.api.Assertions.*;

class TollCalculatorTest {

    @Test
    void getTotalTollFee() {
        // given
        TollCalculator tollCalculator = new TollCalculator();

        // when
        int tollFreeVehicle = tollCalculator.getTotalTollFee(createVehicle("Diplomant"), of(2024, Month.APRIL, 12, 0, 0));
        int fee202404050000 = tollCalculator.getTotalTollFee(createVehicle("Car"), of(2024, Month.APRIL, 5, 0, 0));
        int fee202404051530 = tollCalculator.getTotalTollFee(createVehicle("Car"), of(2024, Month.APRIL, 5, 15, 30));
        int fee202404051530_1529 = tollCalculator.getTotalTollFee(createVehicle("Car")
                , of(2024, Month.APRIL, 5, 15, 30)
                , of(2024, Month.APRIL, 5, 15, 29)
        );
        int fee202404050815_1627 = tollCalculator.getTotalTollFee(createVehicle("Car")
                , of(2024, Month.APRIL, 5, 8, 15)
                , of(2024, Month.APRIL, 5, 8, 20)
                , of(2024, Month.APRIL, 5, 16, 27)
                , of(2024, Month.APRIL, 5, 16, 17)
        );
        int fee20240405multi = tollCalculator.getTotalTollFee(createVehicle("Car")
                , of(2024, Month.APRIL, 5, 5, 59) // 0
                , of(2024, Month.APRIL, 5, 8, 15) // 16
                , of(2024, Month.APRIL, 5, 8, 20) // 16
                , of(2024, Month.APRIL, 5, 8, 30) // 9
                , of(2024, Month.APRIL, 5, 9, 30) // 9
                , of(2024, Month.APRIL, 5, 10, 30) // 9
                , of(2024, Month.APRIL, 5, 10, 31) // 9
                , of(2024, Month.APRIL, 5, 10, 32) // 9
                , of(2024, Month.APRIL, 5, 10, 33) // 9
                , of(2024, Month.APRIL, 5, 16, 17) // 22
                , of(2024, Month.APRIL, 5, 16, 27) // 22
                , of(2024, Month.APRIL, 5, 18, 31) // 0
        );


        // then
        assertEquals(0, tollFreeVehicle);
        assertEquals(0, fee202404050000);
        assertEquals(22, fee202404051530);
        assertEquals(22, fee202404051530_1529);
        assertEquals(16 + 22, fee202404050815_1627);
        assertEquals(16 + 9 + 9 + 22, fee20240405multi);
    }

    private static Date of(int year, Month month, int dayOfMonth, int hour, int minute) {
        LocalDateTime ldt = LocalDateTime.of(year, month, dayOfMonth, hour, minute);
        return Date.from(ldt.atZone(ZoneId.systemDefault()).toInstant());
    }

    private static Stream<Arguments> provideDataForIsTollFree() {
        return Stream.of(
                Arguments.of("Car", false),
                Arguments.of("car", false),
                Arguments.of("Diplomat", true),
                Arguments.of("DIPLOMAT", true),
                Arguments.of("Foreign", true),
                Arguments.of("Emergency", true),
                Arguments.of("Military", true),
                Arguments.of("Motorbike", true),
                Arguments.of("Tractor", true),
                Arguments.of("", false),
                Arguments.of(null, false)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsTollFree")
    void isTollFreeVehicle(String vehicleType, boolean expected) {
        // when
        boolean tollFree = TollCalculator.isTollFreeVehicle(createVehicle(vehicleType));

        // then
        assertEquals(expected, tollFree);
    }

    public static Stream<Arguments> provideDataForgetTollFee() {
        return Stream.of(
                Arguments.of(0, 0,   FEEBYINTERVAL.get(TollCalculator.tti00000559)),
                Arguments.of(6, 0,   FEEBYINTERVAL.get(TollCalculator.tti06000629)),
                Arguments.of(6, 59,  FEEBYINTERVAL.get(TollCalculator.tti06300659)),
                Arguments.of(7, 30,  FEEBYINTERVAL.get(TollCalculator.tti07000759)),
                Arguments.of(8, 0,   FEEBYINTERVAL.get(TollCalculator.tti08000829)),
                Arguments.of(12, 0,  FEEBYINTERVAL.get(TollCalculator.tti08301459)),
                Arguments.of(15, 29, FEEBYINTERVAL.get(TollCalculator.tti15001529)),
                Arguments.of(15, 30, FEEBYINTERVAL.get(TollCalculator.tti15301659)),
                Arguments.of(17, 11, FEEBYINTERVAL.get(TollCalculator.tti17001759)),
                Arguments.of(18, 28, FEEBYINTERVAL.get(TollCalculator.tti18001829)),
                Arguments.of(5, 29,  FEEBYINTERVAL.get(TollCalculator.tti00000559))
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForgetTollFee")
    void testGetTollFee(int hour, int minute, int expected) {
        // given
        TollCalculator tollCalculator = new TollCalculator();

        // when
        LocalDateTime localDateTime = LocalDateTime.of(2024, Month.APRIL, 5, hour, minute);
        Date date = Date.from(localDateTime.atZone(ZoneId.systemDefault()).toInstant());

        int fee = tollCalculator.getTollFee(date, createVehicle("Car"));

        // then
        assertEquals(expected, fee);
    }

    public static Stream<Arguments> provideDataForIsTollFreeDate() {
        return Stream.of(
                // Fixed 'Red' days
                Arguments.of(createDate(2024, Calendar.JANUARY, 1), true),
                Arguments.of(createDate(2024, Calendar.MAY, 1), true),
                Arguments.of(createDate(2024, Calendar.JUNE, 6), true),
                Arguments.of(createDate(2024, Calendar.DECEMBER, 25), true),
                Arguments.of(createDate(2024, Calendar.DECEMBER, 26), true),
                Arguments.of(createDate(2024, Calendar.DECEMBER, 31), true),

                // Day before fixed 'Red' day
                Arguments.of(createDate(2024, Calendar.APRIL, 30), true),
                Arguments.of(createDate(2024, Calendar.JUNE, 5), true), // Trettondagsafton
                Arguments.of(createDate(2024, Calendar.DECEMBER, 24), true),

                // 'Red' day
                Arguments.of(createDate(2024, Calendar.JANUARY, 6), true), // Trettondedagen
                Arguments.of(createDate(2024, Calendar.MARCH, 29), true), // Good Friday
                Arguments.of(createDate(2024, Calendar.MARCH, 30), true), // Påskafton
                Arguments.of(createDate(2024, Calendar.MARCH, 31), true), // Påskdagen
                Arguments.of(createDate(2024, Calendar.APRIL, 1), true), // Annandagpåsk
                Arguments.of(createDate(2024, Calendar.APRIL, 2), false), // Annandagpåsk
                Arguments.of(createDate(2024, Calendar.MAY, 9), true), // Kristi himmels
                Arguments.of(createDate(2024, Calendar.MAY, 10), false), // Kristi himmels

                // Day before 'Red' day
                Arguments.of(createDate(2024, Calendar.NOVEMBER, 1), true), // Alla helgon afton
                Arguments.of(createDate(2024, Calendar.MAY, 8), true), // Day before Kristi him
                Arguments.of(createDate(2024, Calendar.MAY, 21), false),
                Arguments.of(createDate(2024, Calendar.NOVEMBER, 11), false),

                // July, Saturday, Sunday
                Arguments.of(createDate(2024, Calendar.JULY, 8), true),
                Arguments.of(createDate(2024, Calendar.AUGUST, 8), false),
                Arguments.of(createDate(2024, Calendar.AUGUST, 24), true),
                Arguments.of(createDate(2024, Calendar.AUGUST, 22), false),
                Arguments.of(createDate(2024, Calendar.SEPTEMBER, 1), true),
                Arguments.of(createDate(2024, Calendar.SEPTEMBER, 2), false)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsTollFreeDate")
    void testIsTollFreeDate(Date date, boolean expected) {
        // when
        boolean isTollFreeDate = TollCalculator.isTollFreeDate(date);

        // then
        assertEquals(expected, isTollFreeDate);
    }

    private static Date createDate(int year, int month, int day) {
        Calendar calendar = Calendar.getInstance();
        calendar.set(year, month, day);

        return calendar.getTime();
    }

    private Vehicle createVehicle(final String type) {
        return () -> type;
    }
}
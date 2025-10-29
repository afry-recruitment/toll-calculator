package com.assignment.toll_calculator.services;

import com.assignment.toll_calculator.models.Vehicle;
import com.assignment.toll_calculator.utils.DateUtils;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import java.util.*;

import static com.assignment.toll_calculator.services.TollService.*;
import static com.assignment.toll_calculator.helpers.TestUtils.composeDate;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

public class TollServiceTest {

    @Test
    public void givenNoVehicleAndValidDates_whenCalculation_thenFailure() {
        // given
        Vehicle vehicle = null;
        List<Date> dates = List.of(composeDate(Calendar.SEPTEMBER, 2));

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class, () -> new TollService().calculateFee(vehicle, dates)
        );

        // then
        assertEquals(INVALID_VEHICLE_ERROR_MESSAGE, exception.getMessage());
    }

    @Test
    public void givenCarVehicleAndNoDates_whenCalculation_thenFailure() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = null;

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class, () -> new TollService().calculateFee(vehicle, dates)
        );

        // then
        assertEquals(INVALID_DATES_ERROR_MESSAGE, exception.getMessage());
    }

    @Test
    public void givenCarVehicleAndEmptyDates_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = Collections.emptyList();

        // when
        int actualTollFee = new TollService().calculateFee(vehicle, dates);

        // then
        assertEquals(0, actualTollFee);
    }

    @Test
    public void givenCarVehicleAndDifferentDayDates_whenCalculation_thenFailure() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(composeDate(Calendar.SEPTEMBER, 2), composeDate(Calendar.SEPTEMBER, 3));

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class, () -> new TollService().calculateFee(vehicle, dates)
        );

        // then
        assertEquals(DIFFERENT_DAYS_ERROR_MESSAGE, exception.getMessage());
    }

    @Test
    public void givenCarVehicleAndSameDayDates_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(
                composeDate(Calendar.SEPTEMBER, 2, 7, 30),
                composeDate(Calendar.SEPTEMBER, 2, 8, 30),
                composeDate(Calendar.SEPTEMBER, 2, 10, 30),
                composeDate(Calendar.SEPTEMBER, 2, 12, 30),
                composeDate(Calendar.SEPTEMBER, 2, 14, 30),
                composeDate(Calendar.SEPTEMBER, 2, 16, 30),
                composeDate(Calendar.SEPTEMBER, 2, 18, 29)
        );

        // when
        int actualTollFee = new TollService().calculateFee(vehicle, dates);

        // then
        assertEquals(60, actualTollFee);
    }

    @Test
    public void givenMotorbikeVehicleAndSameDayDates_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.MOTORBIKE;
        List<Date> dates = List.of(
                composeDate(Calendar.SEPTEMBER, 2, 7, 30),
                composeDate(Calendar.SEPTEMBER, 2, 8, 30),
                composeDate(Calendar.SEPTEMBER, 2, 10, 30),
                composeDate(Calendar.SEPTEMBER, 2, 12, 30),
                composeDate(Calendar.SEPTEMBER, 2, 14, 30),
                composeDate(Calendar.SEPTEMBER, 2, 16, 30),
                composeDate(Calendar.SEPTEMBER, 2, 18, 29)
        );

        // when
        int actualTollFee = new TollService().calculateFee(vehicle, dates);

        // then
        assertEquals(0, actualTollFee);
    }

    @Test
    public void givenCarVehicleAndHoliday_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(composeDate(Calendar.JANUARY, 1));

        // when
        int actualTollFee = new TollService().calculateFee(vehicle, dates);

        // then
        assertEquals(0, actualTollFee);
    }

    @ParameterizedTest
    @CsvSource(
            value = {
                    "2013-09-02T05:29:00,0",  "2013-09-02T05:30:00,0",
                    "2013-09-02T06:29:00,8",  "2013-09-02T06:30:00,13",
                    "2013-09-02T07:29:00,18", "2013-09-02T07:30:00,18",
                    "2013-09-02T08:29:00,13", "2013-09-02T08:30:00,8",
                    "2013-09-02T09:29:00,8",  "2013-09-02T09:30:00,8",
                    "2013-09-02T10:29:00,8",  "2013-09-02T10:30:00,8",
                    "2013-09-02T11:29:00,8",  "2013-09-02T11:30:00,8",
                    "2013-09-02T12:29:00,8",  "2013-09-02T12:30:00,8",
                    "2013-09-02T13:29:00,8",  "2013-09-02T13:30:00,8",
                    "2013-09-02T14:29:00,8",  "2013-09-02T14:30:00,8",
                    "2013-09-02T15:29:00,13", "2013-09-02T15:30:00,18",
                    "2013-09-02T16:29:00,18", "2013-09-02T16:30:00,18",
                    "2013-09-02T17:29:00,13", "2013-09-02T17:30:00,13",
                    "2013-09-02T18:29:00,8",  "2013-09-02T18:30:00,0",
                    "2013-09-02T19:29:00,0",  "2013-09-02T19:30:00,0",
            }
    )
    public void givenCarVehicleAndEachHourDates_whenCalculation_thenSuccess(String source, String expectedTollFee) {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(DateUtils.convert(source, DateUtils.DATE_TIME_PATTERN));

        // when
        int actualTollFee = new TollService().calculateFee(vehicle, dates);

        // then
        assertEquals(Integer.parseInt(expectedTollFee), actualTollFee);
    }

    @ParameterizedTest
    @CsvSource(
            value = {
                    "2013-09-02T06:30:00,2013-09-02T06:31:00,2013-09-02T06:32:00;13",
                    "2013-09-02T06:29:00,2013-09-02T06:59:00,2013-09-02T07:25:00;18",
                    "2013-09-02T06:29:00,2013-09-02T06:59:00,2013-09-02T07:55:00;31",
                    "2013-09-02T06:29:00,2013-09-02T06:59:00,2013-09-02T07:55:00,2013-09-02T06:59:00;31",
                    "2013-09-02T06:29:00,2013-09-02T06:59:00,2013-09-02T07:55:00,2013-09-02T07:56:00;31",
                    "2013-09-02T06:29:00,2013-09-02T06:59:00,2013-09-02T08:00:00;26",
                    "2013-09-02T15:00:00,2013-09-02T15:00:00;13",
                    "2013-09-02T15:00:00,2013-09-02T16:00:00;18",
                    "2013-09-02T15:00:00,2013-09-02T16:00:01;31",
                    "2013-09-02T15:00:00,2013-09-02T16:01:00;31",
                    "2013-09-02T15:30:00,2013-09-02T17:40:00,2013-09-02T18:20:00;31",
                    "2013-09-02T17:40:00,2013-09-02T18:20:00,2013-09-02T15:30:00;31",
            },
            delimiterString = ";"
    )
    public void givenCarVehicleAndHourPeriodDates_whenCalculation_thenSuccess(String source, String expectedTollFee) {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = new ArrayList<>(Arrays.asList(source.split(",")))
                .stream()
                .map(date -> DateUtils.convert(date, DateUtils.DATE_TIME_PATTERN))
                .toList();

        // when
        int actualTollFee = new TollService().calculateFee(vehicle, dates);

        // then
        assertEquals(Integer.parseInt(expectedTollFee), actualTollFee);
    }

}

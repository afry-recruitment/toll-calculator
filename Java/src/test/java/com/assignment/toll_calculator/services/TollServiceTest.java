package com.assignment.toll_calculator.services;

import com.assignment.toll_calculator.models.Vehicle;
import com.assignment.toll_calculator.utils.DateUtils;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import java.util.Calendar;
import java.util.Collections;
import java.util.Date;
import java.util.List;

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
                composeDate(Calendar.SEPTEMBER, 2, 8, 30),
                composeDate(Calendar.SEPTEMBER, 2, 16, 30)
        );

        // when
        int actualTollFee = new TollService().calculateFee(vehicle, dates);

        // then
        assertEquals(26, actualTollFee);
    }

    @Test
    public void givenMotorbikeVehicleAndSameDayDates_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.MOTORBIKE;
        List<Date> dates = List.of(
                composeDate(Calendar.SEPTEMBER, 2, 8, 30),
                composeDate(Calendar.SEPTEMBER, 2, 16, 30)
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
    public void givenCarVehicleAndParameterizedDates_whenCalculation_thenSuccess(String date, String expectedTollFee) {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(DateUtils.convert(date, DateUtils.DATE_TIME_PATTERN));

        // when
        int actualTollFee = new TollService().calculateFee(vehicle, dates);

        // then
        assertEquals(Integer.parseInt(expectedTollFee), actualTollFee);
    }

}

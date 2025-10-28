package com.assignment.toll_calculator.services;

import com.assignment.toll_calculator.models.Vehicle;
import org.junit.jupiter.api.Test;

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
        List<Date> dates = List.of(composeDate(Calendar.SEPTEMBER, 1));

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class, () -> new TollService().getTollFee(vehicle, dates)
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
                IllegalArgumentException.class, () -> new TollService().getTollFee(vehicle, dates)
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
        int actualTollFee = new TollService().getTollFee(vehicle, dates);

        // then
        assertEquals(0, actualTollFee);
    }

    @Test
    public void givenCarVehicleAndDifferentDayDates_whenCalculation_thenFailure() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(composeDate(Calendar.SEPTEMBER, 1), composeDate(Calendar.SEPTEMBER, 2));

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class, () -> new TollService().getTollFee(vehicle, dates)
        );

        // then
        assertEquals(DIFFERENT_DAYS_ERROR_MESSAGE, exception.getMessage());
    }

    @Test
    public void givenCarVehicleAndSameDayDates_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(
                composeDate(Calendar.SEPTEMBER, 1, 8, 30),
                composeDate(Calendar.SEPTEMBER, 1, 16, 30)
        );

        // when
        int actualTollFee = new TollService().getTollFee(vehicle, dates);

        // then
        assertEquals(0, actualTollFee);
    }

    @Test
    public void givenMotorbikeVehicleAndSameDayDates_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.MOTORBIKE;
        List<Date> dates = List.of(
                composeDate(Calendar.SEPTEMBER, 1, 8, 30),
                composeDate(Calendar.SEPTEMBER, 1, 16, 30)
        );

        // when
        int actualTollFee = new TollService().getTollFee(vehicle, dates);

        // then
        assertEquals(0, actualTollFee);
    }

    @Test
    public void givenCarVehicleAndHoliday_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(composeDate(Calendar.JANUARY, 1));

        // when
        int actualTollFee = new TollService().getTollFee(vehicle, dates);

        // then
        assertEquals(0, actualTollFee);
    }

}

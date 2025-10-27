package com.assignment.toll_calculator.services;

import com.assignment.toll_calculator.models.Vehicle;
import org.junit.jupiter.api.Test;

import java.util.Collections;
import java.util.Date;
import java.util.List;

import static com.assignment.toll_calculator.services.TollCalculator.*;
import static com.assignment.toll_calculator.helpers.TestUtils.composeDate;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

public class TollCalculatorTest {

    @Test
    public void givenNoVehicleAndDates_whenCalculation_thenFailure() {
        // given
        Vehicle vehicle = null;
        List<Date> dates = List.of(composeDate(3, 1));

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class,
                () -> new TollCalculator().getTollFee(vehicle, dates)
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
                IllegalArgumentException.class,
                () -> new TollCalculator().getTollFee(vehicle, dates)
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
        int actualTollFee = new TollCalculator().getTollFee(vehicle, dates);

        // then
        assertEquals(0, actualTollFee);
    }

    @Test
    public void givenCarVehicleAndDifferentDays_whenCalculation_thenFailure() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(composeDate(3, 1), composeDate(3, 2));

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class,
                () -> new TollCalculator().getTollFee(vehicle, dates)
        );

        // then
        assertEquals(DIFFERENT_DAYS_ERROR_MESSAGE, exception.getMessage());
    }

    @Test
    public void givenCarVehicleAndSameDays_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(
                composeDate(3, 1, 8, 30),
                composeDate(3, 1, 16, 30)
        );

        // when
        int actualTollFee = new TollCalculator().getTollFee(vehicle, dates);

        // then
        assertEquals(26, actualTollFee);
    }

    @Test
    public void givenCarVehicleAndHoliday_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = Vehicle.CAR;
        List<Date> dates = List.of(composeDate(0, 1));

        // when
        int actualTollFee = new TollCalculator().getTollFee(vehicle, dates);

        // then
        assertEquals(0, actualTollFee);
    }

}

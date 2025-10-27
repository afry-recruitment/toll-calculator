package com.assignment.toll_calculator.services;

import com.assignment.toll_calculator.models.Car;
import com.assignment.toll_calculator.models.Vehicle;
import org.junit.jupiter.api.Test;

import java.util.Date;
import java.util.List;

import static com.assignment.toll_calculator.utils.DateUtils.composeDate;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

public class TollCalculatorTest {

    @Test
    public void givenCarVehicleAndDifferentDays_whenCalculation_thenFailure() {
        // given
        Vehicle vehicle = new Car();
        List<Date> dates = List.of(composeDate(3, 1), composeDate(3, 2));

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class,
                () -> new TollCalculator().getTollFee(vehicle, dates)
        );

        // then
        assertEquals("Cannot identify tolls for different days at the same time!", exception.getMessage());
    }

    @Test
    public void givenCarVehicleAndSameDays_whenCalculation_thenSuccess() {
        // given
        Vehicle vehicle = new Car();
        List<Date> dates = List.of(
                composeDate(3, 1, 8, 30),
                composeDate(3, 1, 16, 30)
        );

        // when
        int tollFee = new TollCalculator().getTollFee(vehicle, dates);

        // then
        assertEquals(0, tollFee);
    }

}

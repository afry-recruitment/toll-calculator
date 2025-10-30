package com.assignment.toll_calculator.services;

import com.assignment.toll_calculator.models.VehicleType;
import com.assignment.toll_calculator.utils.DateUtils;
import com.assignment.toll_calculator.utils.VehicleTypeUtils;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvFileSource;
import org.junit.jupiter.params.provider.CsvSource;

import java.util.*;

import static com.assignment.toll_calculator.services.TollService.*;
import static com.assignment.toll_calculator.helpers.TestUtils.composeDate;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

/**
 * This class contains all test cases of TollService.
 */
public class TollServiceTest {

    @Test
    public void givenNoVehicleTypeAndValidDates_whenCalculation_thenFailure() {
        // given
        VehicleType vehicleType = null;
        List<Date> dates = List.of(composeDate(Calendar.SEPTEMBER, 2));

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class, () -> new TollService().calculateFee(vehicleType, dates)
        );

        // then
        assertEquals(INVALID_VEHICLE_TYPE_ERROR_MESSAGE, exception.getMessage());
    }

    @Test
    public void givenCarVehicleTypeAndNoDates_whenCalculation_thenFailure() {
        // given
        VehicleType vehicleType = VehicleType.CAR;
        List<Date> dates = null;

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class, () -> new TollService().calculateFee(vehicleType, dates)
        );

        // then
        assertEquals(INVALID_DATES_ERROR_MESSAGE, exception.getMessage());
    }

    @Test
    public void givenCarVehicleTypeAndEmptyDates_whenCalculation_thenSuccess() {
        // given
        VehicleType vehicleType = VehicleType.CAR;
        List<Date> dates = Collections.emptyList();

        // when
        int actualTollFee = new TollService().calculateFee(vehicleType, dates);

        // then
        assertEquals(0, actualTollFee);
    }

    @Test
    public void givenCarVehicleTypeAndDifferentDayDates_whenCalculation_thenFailure() {
        // given
        VehicleType vehicleType = VehicleType.CAR;
        List<Date> dates = List.of(composeDate(Calendar.SEPTEMBER, 2), composeDate(Calendar.SEPTEMBER, 3));

        // when
        Exception exception = assertThrows(
                IllegalArgumentException.class, () -> new TollService().calculateFee(vehicleType, dates)
        );

        // then
        assertEquals(DIFFERENT_DAYS_ERROR_MESSAGE, exception.getMessage());
    }

    @Test
    public void givenCarVehicleTypeAndSameDayDates_whenCalculation_thenSuccess() {
        // given
        VehicleType vehicleType = VehicleType.CAR;
        List<Date> dates = List.of(
                composeDate(Calendar.SEPTEMBER, 2, 7, 30),
                composeDate(Calendar.SEPTEMBER, 2, 8, 30)
        );

        // when
        int actualTollFee = new TollService().calculateFee(vehicleType, dates);

        // then
        assertEquals(18, actualTollFee);
    }

    @Test
    public void givenCarVehicleTypeAndValidDates_whenCalculationAndDailyTotalFeeReached_thenSuccess() {
        // given
        VehicleType vehicleType = VehicleType.CAR;
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
        int actualTollFee = new TollService().calculateFee(vehicleType, dates);

        // then
        assertEquals(60, actualTollFee);
    }

    @Test
    public void givenMotorbikeVehicleTypeAndValidDates_whenCalculation_thenSuccess() {
        // given
        VehicleType vehicleType = VehicleType.MOTORBIKE;
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
        int actualTollFee = new TollService().calculateFee(vehicleType, dates);

        // then
        assertEquals(0, actualTollFee);
    }

    @Test
    public void givenCarVehicleTypeAndHoliday_whenCalculation_thenSuccess() {
        // given
        VehicleType vehicleType = VehicleType.CAR;
        List<Date> dates = List.of(composeDate(Calendar.JANUARY, 1));

        // when
        int actualTollFee = new TollService().calculateFee(vehicleType, dates);

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
    public void givenCarVehicleTypeAndEachHourDates_whenCalculation_thenSuccess(String source, String expectedTollFee) {
        // given
        VehicleType vehicleType = VehicleType.CAR;
        List<Date> dates = List.of(DateUtils.convert(source, DateUtils.DATE_TIME_PATTERN));

        // when
        int actualTollFee = new TollService().calculateFee(vehicleType, dates);

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
    public void givenCarVehicleTypeAndHourPeriodDates_whenCalculation_thenSuccess(
            String source, String expectedTollFee) {

        // given
        VehicleType vehicleType = VehicleType.CAR;
        List<Date> dates = new ArrayList<>(Arrays.asList(source.split(",")))
                .stream()
                .map(date -> DateUtils.convert(date, DateUtils.DATE_TIME_PATTERN))
                .toList();

        // when
        int actualTollFee = new TollService().calculateFee(vehicleType, dates);

        // then
        assertEquals(Integer.parseInt(expectedTollFee), actualTollFee);

    }

    @ParameterizedTest
    @CsvFileSource(
            resources = {
                    "car_2013-06-03_test_scenarios.csv",
                    "car_2013-06-06_test_scenarios.csv",
                    "car_2013-06-08_test_scenarios.csv",
                    "car_2013-06-09_test_scenarios.csv",
                    "motorbike_2013-06-03_test_scenarios.csv",
                    "motorbike_2013-06-06_test_scenarios.csv",
                    "motorbike_2013-06-08_test_scenarios.csv",
                    "motorbike_2013-06-09_test_scenarios.csv"
            },
            numLinesToSkip = 1,
            delimiterString = "|"
    )
    // For details about the meaning of t1...t11 variables, please read Java/README.md.
    public void givenDifferentTimePeriodsAndVehicleTypesAndDates_whenCalculationWith_thenSuccess(
            String vehicleType, String day,
            String t1, String t2, String t3, String t4, String t5,
            String t6,String t7, String t8, String t9, String t10, String t11,
            String expectedTollFee) {

        // given
        VehicleType parsedVehicleType = VehicleTypeUtils.parse(vehicleType);

        List<Date> dates = new ArrayList<>();
        if (Boolean.parseBoolean(t1)) dates.add(composeDate(day, 4, 30));
        if (Boolean.parseBoolean(t2)) dates.add(composeDate(day, 6, 0));
        if (Boolean.parseBoolean(t3)) dates.add(composeDate(day, 6, 30));
        if (Boolean.parseBoolean(t4)) dates.add(composeDate(day, 7, 31));
        if (Boolean.parseBoolean(t5)) dates.add(composeDate(day, 8, 29));
        if (Boolean.parseBoolean(t6)) dates.add(composeDate(day, 9, 30));
        if (Boolean.parseBoolean(t7)) dates.add(composeDate(day, 15, 29));
        if (Boolean.parseBoolean(t8)) dates.add(composeDate(day, 16, 30));
        if (Boolean.parseBoolean(t9)) dates.add(composeDate(day, 17, 31));
        if (Boolean.parseBoolean(t10)) dates.add(composeDate(day, 18, 29));
        if (Boolean.parseBoolean(t11)) dates.add(composeDate(day, 19, 30));

        // when
        int actualTollFee = new TollService().calculateFee(parsedVehicleType, dates);

        // then
        assertEquals(Integer.parseInt(expectedTollFee), actualTollFee);
    }

}

package com.assignment.toll_calculator.controllers;

import com.assignment.toll_calculator.helpers.TestUtils;
import com.assignment.toll_calculator.models.VehicleType;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.ResultActions;

import java.util.*;
import java.util.stream.Stream;

import static com.assignment.toll_calculator.helpers.TestUtils.composeDate;
import static com.assignment.toll_calculator.utils.ConstantUtils.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

/**
 * This class contains all test cases for the toll fee REST endpoint.
 */
@SpringBootTest
@AutoConfigureMockMvc
public class TollControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Test
    public void givenNoVehicleTypeAndValidDate_whenRequest_thenFailure() throws Exception {
        // given
        String vehicleType = null;
        String dates = String.join(
                DATES_REQUEST_PARAM_VALUE_DELIMITER,
                Stream.of(composeDate(Calendar.SEPTEMBER, 2)).map(TestUtils::format).toList()
        );

        // when
        ResultActions resultActions = mockMvc.perform(
                get(FEE_URL)
                        .param(VEHICLE_TYPE_REQUEST_PARAM, vehicleType)
                        .param(DATES_REQUEST_PARAM, dates)
        );

        // then
        resultActions
                .andExpect(status().isBadRequest())
                .andExpect(
                        jsonPath("$.message")
                                .value(
                                        String.format(
                                                "Required request parameter '%s' for method " +
                                                "parameter type String is not present",
                                                VEHICLE_TYPE_REQUEST_PARAM
                                        )
                                )
                );
    }

    @Test
    public void givenEmptyVehicleTypeAndValidDate_whenRequest_thenFailure() throws Exception {
        // given
        String vehicleType = "";
        String dates = String.join(
                DATES_REQUEST_PARAM_VALUE_DELIMITER,
                Stream.of(composeDate(Calendar.SEPTEMBER, 2)).map(TestUtils::format).toList()
        );

        // when
        ResultActions resultActions = mockMvc.perform(
                get(FEE_URL)
                        .param(VEHICLE_TYPE_REQUEST_PARAM, vehicleType)
                        .param(DATES_REQUEST_PARAM, dates)
        );

        // then
        resultActions
                .andExpect(status().isBadRequest())
                .andExpect(
                        jsonPath("$.message")
                                .value("calculateFee.vehicleType: The vehicle type is required!")
                );
    }

    @Test
    public void givenInvalidVehicleTypeAndValidDate_whenRequest_thenFailure() throws Exception {
        // given
        String vehicleType = "some-vehicle-type";
        String dates = String.join(
                DATES_REQUEST_PARAM_VALUE_DELIMITER,
                Stream.of(composeDate(Calendar.SEPTEMBER, 2)).map(TestUtils::format).toList()
        );

        // when
        ResultActions resultActions = mockMvc.perform(
                get(FEE_URL)
                        .param(VEHICLE_TYPE_REQUEST_PARAM, vehicleType)
                        .param(DATES_REQUEST_PARAM, dates)
        );

        // then
        resultActions
                .andExpect(status().isBadRequest())
                .andExpect(
                        jsonPath("$.message")
                                .value(String.format("Cannot parse vehicle type based on %s", vehicleType))
                );
    }

    @Test
    public void givenValidVehicleTypeAndNoDate_whenRequest_thenFailure() throws Exception {
        // given
        String vehicleType = VehicleType.FOREIGN.getType();
        String dates = null;

        // when
        ResultActions resultActions = mockMvc.perform(
                get(FEE_URL)
                        .param(VEHICLE_TYPE_REQUEST_PARAM, vehicleType)
                        .param(DATES_REQUEST_PARAM, dates)
        );

        // then
        resultActions
                .andExpect(status().isBadRequest())
                .andExpect(
                        jsonPath("$.message")
                                .value(
                                        String.format(
                                                "Required request parameter '%s' for method " +
                                                        "parameter type String is not present",
                                                DATES_REQUEST_PARAM
                                        )
                                )
                );
    }

    @Test
    public void givenValidVehicleTypeAndEmptyDate_whenRequest_thenFailure() throws Exception {
        // given
        String vehicleType = VehicleType.FOREIGN.getType();
        String dates = "";

        // when
        ResultActions resultActions = mockMvc.perform(
                get(FEE_URL)
                        .param(VEHICLE_TYPE_REQUEST_PARAM, vehicleType)
                        .param(DATES_REQUEST_PARAM, dates)
        );

        // then
        resultActions
                .andExpect(status().isBadRequest())
                .andExpect(
                        jsonPath("$.message")
                                .value("calculateFee.dates: At least one date is required!")
                );
    }

}

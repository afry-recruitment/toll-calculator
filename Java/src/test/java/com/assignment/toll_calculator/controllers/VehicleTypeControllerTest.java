package com.assignment.toll_calculator.controllers;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.ResultActions;

import static com.assignment.toll_calculator.utils.ConstantUtils.VEHICLE_TYPES_URL;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

/**
 * This class contains all test cases for the actuator health REST endpoint.
 */
@SpringBootTest
@AutoConfigureMockMvc
public class VehicleTypeControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Test
    public void whenRequest_thenSuccess() throws Exception {
        // when
        ResultActions resultActions = mockMvc.perform(get(VEHICLE_TYPES_URL));

        // then
        resultActions
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.length()").value(7));
    }

}

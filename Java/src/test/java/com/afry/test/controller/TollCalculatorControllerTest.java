package com.afry.test.controller;

import java.util.Date;

import static org.mockito.ArgumentMatchers.any;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.content;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

import org.apache.catalina.security.SecurityConfig;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;
import org.mockito.junit.jupiter.MockitoExtension;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.context.annotation.Import;
import org.springframework.http.MediaType;
import org.springframework.security.test.context.support.WithMockUser;
import org.springframework.test.context.ContextConfiguration;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;
import org.springframework.test.web.servlet.result.MockMvcResultMatchers;

import com.afry.test.model.Car;
import com.afry.test.model.Vehicle;
import com.afry.test.service.TollCalculatorService;
import com.afry.test.service.VehicleFactory;

/**
 * TollControllerTest to cover and test TollController API methods
 */
@ExtendWith(MockitoExtension.class)
@WebMvcTest(TollCalculatorController.class)
@ContextConfiguration
@Import(SecurityConfig.class)
public class TollCalculatorControllerTest {
    @MockBean
    TollCalculatorService tollCalculatorService;
    @Autowired
    MockMvc mockMvc;
    @MockBean
    VehicleFactory vehicleFactory;

    @BeforeEach
    public void init() {
        MockitoAnnotations.openMocks(this);
    }

    @Test
    @WithMockUser(username = "admin", roles = {"ADMIN"}, password = "root")
    @DisplayName("Test TollEntry For Toll Free Vehicle")
    public void testTollEntryForTollFreeVehicle() throws Exception {
        Vehicle vehicleMock = new Car();
        Mockito.when(vehicleFactory.getVehicle(any(String.class))).thenReturn(vehicleMock);
        Mockito.when(tollCalculatorService.isTollFreeVehicle(any(Vehicle.class))).thenReturn(true);

        mockMvc.perform(MockMvcRequestBuilders.post("/calculator")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{\n" +
                                "\t\"vehicleId\":\"CNC12123\",\n" +
                                "\t \"type\":\"Car\"\n" +
                                "}")
                        .accept(MediaType.APPLICATION_JSON))
                .andExpect(status().isOk())
                .andExpect(content().string("CNC12123 is Toll Free Vehicle"));
    }

    @Test
    @WithMockUser(username = "admin", roles = {"ADMIN"}, password = "root")
    @DisplayName("Test Toll Entry For Valid Input")
    public void testTollEntryForValidInput() throws Exception {
        Vehicle vehicleMock = new Car();
        Mockito.when(vehicleFactory.getVehicle(any(String.class))).thenReturn(vehicleMock);
        Mockito.when(tollCalculatorService.isTollFreeVehicle(any(Vehicle.class))).thenReturn(false);
        Mockito.when(tollCalculatorService.isTollFreeDate(any(Date.class))).thenReturn(false);
        Mockito.when(tollCalculatorService.calculateTollFee(any(Vehicle.class))).thenReturn(13);

        mockMvc.perform(MockMvcRequestBuilders.post("/calculator")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{\n" +
                                "\t\"vehicleId\":\"CNC12123\",\n" +
                                "\t \"type\":\"Car\"\n" +
                                "}")
                        .accept(MediaType.APPLICATION_JSON))
                .andExpect(status().isOk())
                .andExpect(MockMvcResultMatchers.jsonPath("$.tollFee").value("13"));
    }

    @Test
    @WithMockUser(username = "admin", roles = {"ADMIN"}, password = "root")
    @DisplayName("Test TollEntry For  Toll Free Day")
    public void testTollEntryForTollFreeDay() throws Exception {
        Vehicle vehicleMock = new Car();
        Mockito.when(vehicleFactory.getVehicle(any(String.class))).thenReturn(vehicleMock);
        Mockito.when(tollCalculatorService.isTollFreeVehicle(any(Vehicle.class))).thenReturn(false);
        Mockito.when(tollCalculatorService.isTollFreeDate(any(Date.class))).thenReturn(true);

        mockMvc.perform(MockMvcRequestBuilders.post("/calculator")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{\n" +
                                "\t\"vehicleId\":\"CNC12123\",\n" +
                                "\t \"type\":\"Car\"\n" +
                                "}")
                        .accept(MediaType.APPLICATION_JSON))
                .andExpect(status().isOk())
                .andExpect(content().string("Today is Toll Free Day"));
    }

    @Test
    @WithMockUser(username = "admin", roles = {"ADMIN"}, password = "root")
    @DisplayName("Test TollEntry For Invalid Input")
    public void testTollEntryForInvalidTypeInput() throws Exception {

        mockMvc.perform(MockMvcRequestBuilders.post("/calculator")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{\n" +
                                "\t\"vehicleId\":\"CNC12123\",\n" +
                                "\t \"type\":\"\"\n" +
                                "}")
                        .accept(MediaType.APPLICATION_JSON))
                .andExpect(status().isBadRequest())
                .andExpect(content().string("TollEntryRequest -Vehicle Type Cannot be Empty"));
    }

    @Test
    @WithMockUser(username = "admin", roles = {"ADMIN"}, password = "root")
    @DisplayName("Test TollEntry For Invalid Vehicle No Input")
    public void testTollEntryForInvalidVehicleNoInput() throws Exception {

        mockMvc.perform(MockMvcRequestBuilders.post("/calculator")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{\n" +
                                "\t\"vehicleId\":\"CNC23\",\n" +
                                "\t \"type\":\"Car\"\n" +
                                "}")
                        .accept(MediaType.APPLICATION_JSON))
                .andExpect(status().isBadRequest())
                .andExpect(content().string("TollEntryRequest -Invalid Vehicle ID - Vehicle Registration ID Should be 8 Characters"));
    }

    @Test
    @DisplayName("Test TollEntry For Invalid User")
    public void testTollEntryForInvalidUser() throws Exception {

        mockMvc.perform(MockMvcRequestBuilders.post("/calculator")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{\n" +
                                "\t\"vehicleId\":\"CNC23\",\n" +
                                "\t \"type\":\"Car\"\n" +
                                "}")
                        .accept(MediaType.APPLICATION_JSON))
                .andExpect(status().isUnauthorized());
    }
}

package com.afry.test.service;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;

import org.mockito.MockitoAnnotations;
import org.mockito.junit.jupiter.MockitoExtension;

import com.afry.test.model.Vehicle;

/**
 * VehicleFactoryTest to cover and test VehicleFactory methods
 */
@ExtendWith(MockitoExtension.class)
public class VehicleFactoryTest {
    VehicleFactory vehicleFactory;

    @BeforeEach
    public void init() {
        MockitoAnnotations.openMocks(this);
        vehicleFactory = new VehicleFactory();
    }

    @Test
    @DisplayName("Test Get Vehicle For Car InputType")
    void testGetVehicleForCarInputType() throws Exception {
        Vehicle vehicle = vehicleFactory.getVehicle("Car");
        Assertions.assertNotNull(vehicle);
        Assertions.assertEquals("Car", vehicle.getType());
    }

    @Test
    @DisplayName("Test Get Vehicle For Null InputType")
    void testGetVehicleForNullInputType() throws Exception {
        Vehicle vehicle = vehicleFactory.getVehicle(null);
        Assertions.assertNull(vehicle);
    }

    @Test
    @DisplayName("Test Get Vehicle For Motorbike InputType")
    void testGetVehicleForMotorbikeInputType() throws Exception {
        Vehicle vehicle = vehicleFactory.getVehicle("Motorbike");
        Assertions.assertNotNull(vehicle);
        Assertions.assertEquals("Motorbike", vehicle.getType());
    }

    @Test
    @DisplayName("Test Get Vehicle For Invalid InputType")
    void testGetVehicleForInvalidInputType() throws Exception {
        Vehicle vehicle = vehicleFactory.getVehicle("SEDEEEFFF");
        Assertions.assertNull(vehicle);
    }
}

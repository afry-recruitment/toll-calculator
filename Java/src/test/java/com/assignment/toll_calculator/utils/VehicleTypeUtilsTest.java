package com.assignment.toll_calculator.utils;

import com.assignment.toll_calculator.models.VehicleType;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

/**
 * This class contains all test cases of VehicleTypeUtils.
 */
public class VehicleTypeUtilsTest {

    @Test
    public void givenValidType_whenParsing_thenSuccess() {
        // given
        String type = VehicleType.EMERGENCY.getType();

        // when
        VehicleType parsedVehicleType = VehicleTypeUtils.parse(type);

        // then
        assertEquals(VehicleType.EMERGENCY, parsedVehicleType);
    }

    @Test
    public void givenInvalidType_whenParsing_thenSuccess() {
        // given
        String type = "some_type";

        // when
        Exception exception = assertThrows(RuntimeException.class, () -> VehicleTypeUtils.parse(type));

        // then
        assertEquals(
                String.format("Cannot parse vehicle type based on %s", type), exception.getMessage()
        );
    }

}

package com.assignment.toll_calculator.utils;

import com.assignment.toll_calculator.models.internal.VehicleType;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.EnumSource;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

/**
 * This class contains all test cases of VehicleTypeUtils.
 */
public class VehicleTypeUtilsTest {

    @ParameterizedTest
    @EnumSource(VehicleType.class)
    public void givenValidType_whenParsing_thenSuccess(VehicleType expectedVehicleType) {
        // given
        String type = expectedVehicleType.getType();

        // when
        VehicleType actualVehicleType = VehicleTypeUtils.parse(type);

        // then
        assertEquals(expectedVehicleType, actualVehicleType);
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

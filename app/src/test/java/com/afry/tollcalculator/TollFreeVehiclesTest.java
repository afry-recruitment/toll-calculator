package com.afry.tollcalculator;

import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.Optional;
import java.util.concurrent.atomic.AtomicBoolean;
import java.util.stream.Stream;

import static org.junit.jupiter.api.Assertions.*;

class TollFreeVehiclesTest {

    private static Stream<Arguments> provideStringsForFromString() {
        return Stream.of(
                Arguments.of("Car", Optional.of(TollFreeVehicles.CAR)),
                Arguments.of("car", Optional.of(TollFreeVehicles.CAR)),
                Arguments.of("Diplomat", Optional.of(TollFreeVehicles.DIPLOMAT)),
                Arguments.of("DIPLOMAT", Optional.of(TollFreeVehicles.DIPLOMAT)),
                Arguments.of("Foreign", Optional.of(TollFreeVehicles.FOREIGN)),
                Arguments.of("Emergency", Optional.of(TollFreeVehicles.EMERGENCY)),
                Arguments.of("Military", Optional.of(TollFreeVehicles.MILITARY)),
                Arguments.of("Motorbike", Optional.of(TollFreeVehicles.MOTORBIKE)),
                Arguments.of("Tractor", Optional.of(TollFreeVehicles.TRACTOR)),
                Arguments.of(null, Optional.empty()),
                Arguments.of("", Optional.empty())
        );
    }

    @ParameterizedTest
    @MethodSource("provideStringsForFromString")
    void fromStringTest(String vehicle, Optional<TollFreeVehicles> expected) {
        assertEquals(expected, TollFreeVehicles.fromString(vehicle));
    }

    private static Stream<Arguments> provideDataForIsTollFree() {
        return Stream.of(
                Arguments.of("Car", Optional.of(TollFreeVehicles.CAR), false),
                Arguments.of("car", Optional.of(TollFreeVehicles.CAR), false),
                Arguments.of("Diplomat", Optional.of(TollFreeVehicles.DIPLOMAT), true),
                Arguments.of("DIPLOMAT", Optional.of(TollFreeVehicles.DIPLOMAT), true),
                Arguments.of("Foreign", Optional.of(TollFreeVehicles.FOREIGN), true),
                Arguments.of("Emergency", Optional.of(TollFreeVehicles.EMERGENCY), true),
                Arguments.of("Military", Optional.of(TollFreeVehicles.MILITARY), true),
                Arguments.of("Motorbike", Optional.of(TollFreeVehicles.MOTORBIKE), true),
                Arguments.of("Tractor", Optional.of(TollFreeVehicles.TRACTOR), true)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsTollFree")
    void isTollFreeTest(String vehicleId, Optional<TollFreeVehicles> vehicle, boolean expected) {
        AtomicBoolean isTollFree = new AtomicBoolean(false);
        vehicle.ifPresent(v -> isTollFree.set(v.isTollFree()));
        assertEquals(expected, isTollFree.get());
    }
}
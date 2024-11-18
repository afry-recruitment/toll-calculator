package com.afry.tollcalculator;

import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.stream.Stream;

import static org.junit.jupiter.api.Assertions.*;

class TollTimeIntervalTest {

    public static Stream<Arguments> provideDataForIsInside() {
        return Stream.of(
                Arguments.of(new TollTimeInterval(6, 0, 6, 29), 6, 0, true),
                Arguments.of(new TollTimeInterval(6, 0, 6, 29), 6, 29, true),
                Arguments.of(new TollTimeInterval(6, 0, 6, 29), 5, 0, false),
                Arguments.of(new TollTimeInterval(6, 0, 6, 29), 6, 30, false)
        );
    }

    @ParameterizedTest
    @MethodSource("provideDataForIsInside")
    void isInside(TollTimeInterval tollTimeInterval, int hour, int minute, boolean expected) {
        assertEquals(expected, tollTimeInterval.isInside(hour, minute));
    }
}
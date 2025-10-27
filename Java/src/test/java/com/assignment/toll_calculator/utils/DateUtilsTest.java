package com.assignment.toll_calculator.utils;

import org.junit.jupiter.api.Test;

import java.util.Calendar;
import java.util.Date;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

public class DateUtilsTest {

    @Test
    public void givenValidSource_whenConversion_thenSuccess() {
        // given
        String source = "2025-10-27";
        Calendar calendar = Calendar.getInstance();

        // when
        Date actualDate = DateUtils.convert(source);
        calendar.setTime(actualDate);

        // then
        assertEquals(2025, calendar.get(Calendar.YEAR));
        assertEquals(Calendar.OCTOBER, calendar.get(Calendar.MONTH));
        assertEquals(27, calendar.get(Calendar.DAY_OF_MONTH));
    }

    @Test
    public void givenInvalidSource_whenConversion_thenSuccess() {
        // given
        String source = "20251027";

        // when
        Exception exception = assertThrows(
                RuntimeException.class,
                () -> DateUtils.convert(source)
        );

        // then
        assertEquals(
                String.format("java.text.ParseException: Unparseable date: \"%s\"", source),
                exception.getMessage()
        );
    }

}

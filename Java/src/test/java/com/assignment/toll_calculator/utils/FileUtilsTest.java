package com.assignment.toll_calculator.utils;

import org.junit.jupiter.api.Test;

import java.util.List;

import static com.assignment.toll_calculator.utils.FileUtils.HOLIDAYS_FILE_PATH;
import static org.junit.jupiter.api.Assertions.*;

public class FileUtilsTest {

    @Test
    public void givenExistingPath_whenLoading_thenSuccess() {
        // when
        List<String> holidays = FileUtils.loadAsList(HOLIDAYS_FILE_PATH);

        // then
        assertNotNull(holidays);
        assertFalse(holidays.isEmpty());
    }

    @Test
    public void givenInvalidSource_whenConversion_thenSuccess() {
        // given
        String path = "so/me.path";

        // when
        Exception exception = assertThrows(RuntimeException.class, () -> FileUtils.loadAsList(path));

        // then
        assertNotNull(exception.getMessage());
    }

}

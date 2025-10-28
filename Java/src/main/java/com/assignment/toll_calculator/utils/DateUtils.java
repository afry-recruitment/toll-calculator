package com.assignment.toll_calculator.utils;

import lombok.AccessLevel;
import lombok.NoArgsConstructor;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * This class contains all the date related functions supporting the solution.
 */
@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class DateUtils {

    public static final String DATE_PATTERN = "yyyy-MM-dd";
    public static final String DATE_TIME_PATTERN = "yyyy-MM-dd'T'HH:mm:ss";

    /**
     * Convert the text based date
     *
     * @param source - the text based date
     * @param pattern - the pattern to use for conversion
     * @return - the converted date
     */
    public static Date convert(String source, String pattern) {
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat(pattern);
        try {
            return simpleDateFormat.parse(source);
        } catch (ParseException e) {
            throw new RuntimeException(e);
        }
    }

    /**
     * Convert the text based date with the default pattern
     *
     * @param source - the text based date
     * @return - the converted date
     */
    public static Date convert(String source) {
        return convert(source, DATE_PATTERN);
    }

}

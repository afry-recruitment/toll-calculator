package com.assignment.toll_calculator.utils;

import lombok.AccessLevel;
import lombok.NoArgsConstructor;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class DateUtils {

    private static final String DATE_PATTERN = "yyyy-MM-dd";

    /**
     * Convert the text based date
     *
     * @param source - the text based date
     * @return - the converted date
     */
    public static Date convert(String source) {
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat(DATE_PATTERN);
        try {
            return simpleDateFormat.parse(source);
        } catch (ParseException e) {
            throw new RuntimeException(e);
        }
    }
}

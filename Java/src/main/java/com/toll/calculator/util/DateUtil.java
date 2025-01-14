package com.toll.calculator.util;

import com.toll.calculator.exception.TollException;
import lombok.extern.slf4j.Slf4j;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Locale;

@Slf4j
public class DateUtil {
    private static final String DATE_FORMAT = "dd-MM-yyyy HH:mm:ss";

    public static Date[] getDates(List<String> dates)  {
        return dates.stream()
                .map(dateString->{
                    try {
                        return getDateFromString(dateString);
                    } catch (TollException e) {
                        throw new RuntimeException(e);
                    }
                })
                .toArray(Date[]::new);
    }

    public static List<String> getDatesString(List<Date> dates)  {
        return dates.stream()
                .map(date -> new SimpleDateFormat(DATE_FORMAT, Locale.ENGLISH).format(date))
                .toList();
    }

    public static Date getDateFromString(String dateStr) throws TollException {
        SimpleDateFormat formatter = new SimpleDateFormat(DATE_FORMAT, Locale.ENGLISH);
        try {
            return formatter.parse(dateStr);
        } catch (ParseException e) {
           log.error("Invalid Date Format: " + dateStr, e);
            throw new TollException("Invalid Date Format for input: " + dateStr);
        }
    }
}

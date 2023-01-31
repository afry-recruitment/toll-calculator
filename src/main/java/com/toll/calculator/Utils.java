package com.toll.calculator;

import de.jollyday.Holiday;

import java.time.LocalDate;
import java.util.*;
import java.util.stream.Collectors;

public class Utils {
    private static final Random rnd = new Random();

    public static final String LOCALE_LANGUAGE = "sv";
    public static final String LOCALE_COUNTRY = "SE";
    public static final String TIMEZONE = "Europe/Stockholm";

    private static final String DATE_REGEX = "\\d{4}\\-(0[1-9]|1[012])\\-(0[1-9]|[12][0-9]|3[01])";
    private static final String DATE_IS_INCLUDED_REGEX = "^.\\d{4}\\-(0[1-9]|1[012])\\-(0[1-9]|[12][0-9]|3[01]).*";

    public static String getRegNumber(boolean allDigits) {
        StringBuilder regNumber = new StringBuilder();
        String randomizedNumberInStr;
        int randomizedNumber;
        if (allDigits) {
            randomizedNumber = rnd.nextInt(1000000);
            randomizedNumberInStr = String.format("%06d", randomizedNumber);
            regNumber.append(randomizedNumberInStr);
        } else {
            for (int i = 0; i < 3; i++) {
                regNumber.append((char) (rnd.nextInt(26) + 'A'));
            }
            randomizedNumber = rnd.nextInt(1000);
            randomizedNumberInStr = String.format("%03d", randomizedNumber);
            regNumber.append(randomizedNumberInStr);
        }
        return regNumber.toString();
    }

    public static Set<LocalDate> getHolidaysAsDates(Set<Holiday> holidays) {
        return holidays != null ? holidays.stream()
                .filter(h -> h != null && h.getDate() != null)
                .map(Holiday::getDate)
                .collect(Collectors.toSet()) : null;
    }

    public static boolean isDatesWithinSameDay(List<Date> dates) {
        if (dates != null && dates.size() == 1) {
            return true;
        } else if (dates == null) {
            return false;
        }
        Calendar calendar1 = Calendar.getInstance();
        Calendar calendar2 = Calendar.getInstance();

        calendar1.setTime(dates.get(0));
        return dates.stream()
                .skip(1)
                .allMatch(d -> {
                    calendar2.setTime(d);
                    return calendar1.get(Calendar.DAY_OF_YEAR) == calendar2.get(Calendar.DAY_OF_YEAR) &&
                            calendar1.get(Calendar.MONTH) == calendar2.get(Calendar.MONTH) &&
                            calendar1.get(Calendar.YEAR) == calendar2.get(Calendar.YEAR);
                });
    }

    public static int randomizeInt(int bound) {
        return rnd.nextInt(bound);
    }
}

package com.afry.tollfreedates;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class Holidays {
    public List<LocalDate> getHolidays(int year) {
        LocalDate easter = getEaster(year);
        LocalDate midsummer = getMidsummer(year);
        LocalDate allSaintsDay = getAllSaintsDay(year);

        List<LocalDate> holidays = new ArrayList<>();
        //Add all fixed holidays
        holidays.add(LocalDate.of(year, 1, 1)); // New Year's Day
        holidays.add(LocalDate.of(year, 1, 5)); // Twelfth Night
        holidays.add(LocalDate.of(year, 1, 6)); // Epiphany
        holidays.add(LocalDate.of(year, 4, 30)); // Walpurgis Night
        holidays.add(LocalDate.of(year, 5, 1)); // International Workers' Day
        holidays.add(LocalDate.of(year, 6, 6)); // National Day of Sweden
        holidays.add(LocalDate.of(year, 12, 24)); // Christmas Eve
        holidays.add(LocalDate.of(year, 12, 25)); // Christmas Day
        holidays.add(LocalDate.of(year, 12, 26)); // Second Day of Christmas
        holidays.add(LocalDate.of(year, 12, 31)); // New Year's Eve
        //Add all movable holidays
        holidays.add(easter.minusDays(2)); // Good Friday
        holidays.add(easter.minusDays(1)); // Holy Saturday
        holidays.add(easter); // Easter Sunday
        holidays.add(easter.plusDays(1)); // Easter Monday
        holidays.add(easter.plusDays(39)); // Ascension Day
        holidays.add(easter.plusDays(48)); // Whitsun Eve
        holidays.add(easter.plusDays(49)); // Pentecost Sunday
        holidays.add(midsummer.minusDays(1)); // Midsummer Eve
        holidays.add(midsummer); // Midsummer's Day
        holidays.add(allSaintsDay.minusDays(1)); // All Saints' Eve
        holidays.add(allSaintsDay); // All Saints' Day

        return holidays;
    }

    public LocalDate getEaster(int year) {
        // Find Easter with Meeus/Jones/Butcher Easter algorithm
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = ((19 * a) + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + (2 * e) + (2 * i) - h - k) % 7;
        int m = (a + (11 * h) + (22 * l)) / 451;
        int n = (h + l - (7 * m) + 114) / 31;
        int p = (h + l - (7 * m) + 114) % 31;

        return LocalDate.of(year, n, p + 1);
    }

    public LocalDate getMidsummer(int year) {
        LocalDate midsummer = (LocalDate.of(2013, 6, 22));
        List<LocalDate> midsummerWeek = new ArrayList<>(Arrays.asList(
                (LocalDate.of(year, 6, 20)),
                (LocalDate.of(year, 6, 21)),
                (LocalDate.of(year, 6, 22)),
                (LocalDate.of(year, 6, 23)),
                (LocalDate.of(year, 6, 24)),
                (LocalDate.of(year, 6, 25)),
                (LocalDate.of(year, 6, 26))
        ));

        for (LocalDate day : midsummerWeek) {
            if (day.getDayOfWeek() == DayOfWeek.SATURDAY) {
                midsummer = day;
            }
        }

        return midsummer;
    }

    public LocalDate getAllSaintsDay(int year) {
        LocalDate allSaintsDay = (LocalDate.of(2013, 11, 1));
        List<LocalDate> allSaintsDayWeek = new ArrayList<>(Arrays.asList(
                (LocalDate.of(year, 10, 31)),
                (LocalDate.of(year, 11, 1)),
                (LocalDate.of(year, 11, 2)),
                (LocalDate.of(year, 11, 3)),
                (LocalDate.of(year, 11, 4)),
                (LocalDate.of(year, 11, 5)),
                (LocalDate.of(year, 11, 6))
        ));

        for (LocalDate day : allSaintsDayWeek) {
            if (day.getDayOfWeek() == DayOfWeek.SATURDAY) {
                allSaintsDay = day;
            }
        }

        return allSaintsDay;
    }
}

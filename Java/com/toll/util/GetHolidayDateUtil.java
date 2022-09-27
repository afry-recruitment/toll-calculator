package com.toll.util;

import java.io.File;
import java.io.IOException;
import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.util.Scanner;

public class GetHolidayDateUtil {
    private GetHolidayDateUtil() {
    }
    public static Boolean isTollFreeHolidayDate(LocalDateTime date, int years) throws IOException {

        String year = String.valueOf(years);
        String month = String.valueOf(date.getMonthValue());
        if(month.length() == 1) month = "0"+month;
        String day = String.valueOf(date.getDayOfMonth());
        if(day.length() == 1) day = "0"+day;

        File file = new File("E:\\Repos\\toll-calculator\\Java\\holidaylist.txt");
        Scanner sc = new Scanner(file);

        String datesplit= sc.next();
        String[] splits = datesplit.split(",");
        for(String splits2: splits) {
            String holidayDay=year+"-"+month+"-"+day;
            if(holidayDay.equals(splits2)){
                return true;
            }
        }

        DayOfWeek dayOfWeek = date.getDayOfWeek();
        if (dayOfWeek == DayOfWeek.SATURDAY || dayOfWeek == DayOfWeek.SUNDAY)
            return true;

        return false;
    }
}

package com.city.tollcalculator;

import java.util.HashMap;
import java.util.Map;

public class TollRates {
    public static final int MAX_DAILY_FEE = 60;

    public static final Map<TimeRange, Integer> TIME_TO_FEE_MAP = new HashMap<>();

    static {
        TIME_TO_FEE_MAP.put(new TimeRange(6, 0, 6, 29), 8);
        TIME_TO_FEE_MAP.put(new TimeRange(6, 30, 6, 59), 13);
        TIME_TO_FEE_MAP.put(new TimeRange(7, 0, 7, 59), 18);
        TIME_TO_FEE_MAP.put(new TimeRange(8, 0, 8, 29), 13);
        TIME_TO_FEE_MAP.put(new TimeRange(8, 30, 14, 59), 8);
        TIME_TO_FEE_MAP.put(new TimeRange(15, 0, 15, 29), 13);
        TIME_TO_FEE_MAP.put(new TimeRange(15, 30, 16, 59), 18);
        TIME_TO_FEE_MAP.put(new TimeRange(17, 0, 17, 59), 13);
        TIME_TO_FEE_MAP.put(new TimeRange(18, 0, 18, 29), 8);
    }
}

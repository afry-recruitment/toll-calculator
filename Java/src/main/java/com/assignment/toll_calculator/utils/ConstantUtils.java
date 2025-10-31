package com.assignment.toll_calculator.utils;

import lombok.AccessLevel;
import lombok.NoArgsConstructor;

/**
 * This class contains the constants so that it is easier to manage them.
 */
@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class ConstantUtils {

    public static final String FEE_URL = "/fee";

    public static final String VEHICLE_TYPE_REQUEST_PARAM = "vehicle_type";
    public static final String DATES_REQUEST_PARAM = "dates";

    public static final String DATES_REQUEST_PARAM_VALUE_DELIMITER = ",";

    public static final String INVALID_VEHICLE_TYPE_ERROR_MESSAGE = "The vehicle type is required!";
    public static final String INVALID_DATES_ERROR_MESSAGE = "At least one date is required!";
    public static final String DIFFERENT_DAYS_ERROR_MESSAGE =
            "Cannot identify toll fee for dates from different days at the same time!";

    public static final Integer MAX_TOTAL_FEE = 60;

    public static final String DATE_PATTERN = "yyyy-MM-dd";
    public static final String DATE_TIME_PATTERN = "yyyy-MM-dd'T'HH:mm:ss";

    public static final String HOLIDAYS_FILE_PATH = "src/main/resources/data/holidays.json";

}

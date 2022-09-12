package calculator;


import calculator.calendar.CalendarService;
import calculator.calendar.CalendarServiceInterface;
import calculator.vehicles.VehicleType;
import lombok.extern.slf4j.Slf4j;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.ZonedDateTime;
import java.time.temporal.ChronoUnit;
import java.util.*;

@Slf4j
public class TollCalculator
{
    private final HashSet<VehicleType> tollFreeVehicleTypes;
    private final CalendarServiceInterface calendarService;

    public TollCalculator(CalendarServiceInterface calendarService)
    {
        this.tollFreeVehicleTypes = new HashSet<>();
        this.calendarService = calendarService;
        loadTollFreeVehicles();
    }

    private void loadTollFreeVehicles()
    {
        Properties properties =
                PropertiesService.getProperties(PropertiesService.TOOL_FREE_VEHICLES_PROPERTIES);
        Set<String> propertySet = properties.stringPropertyNames();
        boolean parsedAtLeastOne = false;
        for (String property : propertySet)
        {
            boolean isToolFree = Boolean.parseBoolean(properties.getProperty(property));
            if (isToolFree)
            {
                try
                {
                    tollFreeVehicleTypes.add(VehicleType.valueOf(property));
                    parsedAtLeastOne = true;
                } catch (IllegalArgumentException ex)
                {
                    log.error("Miss match between property name in " +
                              PropertiesService.TOOL_FREE_VEHICLES_PROPERTIES + " and the enum name. " +
                              property + " could not be matched. " + ex.getMessage());
                }
            }
        }
        if (parsedAtLeastOne) log.info("Successfully parsed toll free vehicles. ");
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicleType - the vehicle
     * @param dates       - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int getTollFee(VehicleType vehicleType, List<ZonedDateTime> dates)
    {

        ZonedDateTime intervalStart = dates.get(0);
        int totalFee = 0;
        for (ZonedDateTime date : dates)
        {
            int nextFee = getHourlyRate(date, vehicleType);
            int tempFee = getHourlyRate(intervalStart, vehicleType);
            long minutes = ChronoUnit.MINUTES.between(intervalStart, date);
            if (minutes <= 60)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private int getHourlyRate(final ZonedDateTime dateTime, VehicleType vehicleType)
    {
        if (isTollFreeDate(dateTime.toLocalDate()) || tollFreeVehicleTypes.contains(vehicleType)) return 0;
        int hour = dateTime.getHour();
        int minute = dateTime.getMinute();
        if (hour == 6 && minute <= 29) return 8;
        else if (hour == 6) return 13;
        else if (hour == 7) return 18;
        else if (hour == 8 && minute <= 29) return 13;
        else if ((hour >= 8 && minute >= 30) && hour <= 14) return 8;
        else if (hour == 15 && minute <= 29) return 13;
        else if (hour == 15 || hour == 16) return 18;
        else if (hour == 17) return 13;
        else if (hour == 18 && minute <= 29) return 8;
        else return 0;
    }

    private Boolean isTollFreeDate(LocalDate date)
    {
        return calendarService.isWeekend(date) || calendarService.isHoliday(date);
    }
}


package calculator;



import calculator.vehicles.VehicleType;
import lombok.extern.slf4j.Slf4j;

import java.util.*;
import java.util.concurrent.*;

@Slf4j
public class TollCalculator
{
    private final HashSet<VehicleType> tollFreeVehicleTypes;

    public TollCalculator()
    {
        this.tollFreeVehicleTypes = new HashSet<>();
    }

    private void loadTollFreeVehicles()
    {
        Properties properties =
                PropertiesAccessor.getProperties(PropertiesAccessor.TOOL_FREE_VEHICLES_PROPERTIES);
        Set<String> propertySet = properties.stringPropertyNames();
        for (String property : propertySet)
        {
            String[] keyValue = property.split("=");
            if (keyValue.length != 2) continue;
            String vehicleTypeName = keyValue[0];
            boolean isToolFree = Boolean.parseBoolean(keyValue[1]);
            if (isToolFree)
            {
                try
                {
                    tollFreeVehicleTypes.add(VehicleType.valueOf(vehicleTypeName));
                } catch (IllegalArgumentException ex)
                {
                    log.error("Miss match between property name in " +
                              PropertiesAccessor.TOOL_FREE_VEHICLES_PROPERTIES + " and the enum name. " +
                              vehicleTypeName + " could not be matched. " + ex.getMessage());
                }
            }
        }
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicleType - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int getTollFee(VehicleType vehicleType, Date... dates)
    {

        Date intervalStart = dates[0];
        int totalFee = 0;
        for (Date date : dates)
        {
            int nextFee = getHourlyRate(date, vehicleType);
            int tempFee = getHourlyRate(intervalStart, vehicleType);

            TimeUnit timeUnit = TimeUnit.MINUTES;
            long diffInMillies = date.getTime() - intervalStart.getTime();
            long minutes = timeUnit.convert(diffInMillies, TimeUnit.MILLISECONDS);

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

/*  private boolean isTollFreeVehicle(Vehicle vehicle) {
switch (vehicle){
  case TollFreeVehicles.MOTORBIKE -> ""
}
    if(vehicle == null) return false;
    String vehicleType = vehicle.getType();
    return vehicleType.equals(TollFreeVehicles.MOTORBIKE.getType()) ||
           vehicleType.equals(TollFreeVehicles.TRACTOR.getType()) ||
           vehicleType.equals(TollFreeVehicles.EMERGENCY.getType()) ||
           vehicleType.equals(TollFreeVehicles.DIPLOMAT.getType()) ||
           vehicleType.equals(TollFreeVehicles.FOREIGN.getType()) ||
           vehicleType.equals(TollFreeVehicles.MILITARY.getType());
  }*/

    public int getHourlyRate(final Date date, VehicleType vehicleType)
    {
        if (isTollFreeDate(date) || tollFreeVehicleTypes.contains(vehicleType)) return 0;
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        int hour = calendar.get(Calendar.HOUR_OF_DAY);
        int minute = calendar.get(Calendar.MINUTE);

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

    private Boolean isTollFreeDate(Date date)
    {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DAY_OF_MONTH);

        int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
        if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) return true;

        if (year == 2013)
        {
            if (month == Calendar.JANUARY && day == 1 ||
                month == Calendar.MARCH && (day == 28 || day == 29) ||
                month == Calendar.APRIL && (day == 1 || day == 30) ||
                month == Calendar.MAY && (day == 1 || day == 8 || day == 9) ||
                month == Calendar.JUNE && (day == 5 || day == 6 || day == 21) || month == Calendar.JULY ||
                month == Calendar.NOVEMBER && day == 1 ||
                month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
    }
}


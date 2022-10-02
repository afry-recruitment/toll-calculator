package calculator;

import calculator.calendar.CalendarService;
import calculator.fees.*;
import calculator.result.Result;
import calculator.result.ResultFactory;
import calculator.result.ResultImpl;
import calculator.tollrate.TollRateService;
import calculator.vehicle.Vehicle;
import calculator.vehicle.VehicleService;
import calculator.vehicle.VehicleType;
import lombok.extern.slf4j.Slf4j;

import java.time.*;
import java.util.*;

@Slf4j
public record TollCalculator(CalendarService calendarService, VehicleService vehicleService,
                             TollRateService tollRateService)
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle    - String name of VehicleType
     * @param tollPasses - date and time of all passes
     * @return - the total toll fee for that day
     */
    public Result getTollFee(String vehicle, List<ZonedDateTime> tollPasses)
    {
        return getTollFee(new Vehicle(vehicleService.getVehicleType(vehicle), "NO REG ID"), tollPasses);
    }


    /**
     * Calculate the total toll fee given period. Note that if the there is a toll passage within the
     * trailing hour or day intervals of the last intervals in the given tollPasses - then they will
     * probably not be priced correctly unless verified against returned data here.
     *
     * @param vehicle    - the vehicle
     * @param tollPasses - date and time of all passes
     * @return - result detailing all passages with all rates paid
     */
    public Result getTollFee(final Vehicle vehicle, final List<ZonedDateTime> tollPasses)
    {
        final List<ZonedDateTime> sortedTollPasses = new ArrayList<>(tollPasses);
        sortedTollPasses.sort(Comparator.naturalOrder());

        int maxDailyFee = 60;
        HourFee hourFee = new HourFee(sortedTollPasses.get(0));
        DayFee dayFee = new DayFee(sortedTollPasses.get(0), maxDailyFee);
        final List<DayFee> passesByDays = new ArrayList<>();
        int feeAtTime;
        for (final ZonedDateTime tollPass : sortedTollPasses)
        {
            feeAtTime = getHourlyRate(vehicle.getType(), tollPass);
            if (hourFee.contains(tollPass))
            {
                hourFee.addFee(tollPass, feeAtTime);
            }
            else
            {
                dayFee.add(hourFee);
                hourFee = new HourFee(tollPass, feeAtTime);
                if (!dayFee.contains(tollPass))
                {
                    passesByDays.add(dayFee);
                    dayFee = new DayFee(tollPass, maxDailyFee);
                }
            }
        }
        // we have not added hourfee to any dayfee
        if (!hourFee.isEmpty())
        {
            if (dayFee.contains(hourFee.getStart()))
            {
                dayFee.add(hourFee);
            }
            else
            {
                passesByDays.add(dayFee);
                dayFee = new DayFee(hourFee, maxDailyFee);
            }
        }
        if (!dayFee.isEmpty()) passesByDays.add(dayFee);
        return ResultFactory.getResult(passesByDays, vehicle, ZonedDateTime.now());
    }

    private int getHourlyRate(VehicleType vehicleType, final ZonedDateTime dateTime)
    {
        if (isTollFreeDate(dateTime.toLocalDate()) || vehicleType.isTollFree()) return 0;
        return this.tollRateService.getTollFeeAtTime(dateTime.toLocalTime());
    }

    private Boolean isTollFreeDate(LocalDate date)
    {
        return calendarService.isWeekend(date) || calendarService.isHoliday(date);
    }
}


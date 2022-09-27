package calculator;

import calculator.calendar.CalendarService;
import calculator.fees.*;
import calculator.vehicle.Vehicle;
import calculator.vehicle.VehicleService;
import calculator.vehicle.VehicleType;
import lombok.extern.slf4j.Slf4j;

import java.time.*;
import java.time.temporal.ChronoUnit;
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
     * @return - the total toll fee for that day
     */
    public Result getTollFee(Vehicle vehicle, List<ZonedDateTime> tollPasses)
    {

        final List<ZonedDateTime> sortedTollPasses = new ArrayList<>(tollPasses);
        sortedTollPasses.sort(Comparator.naturalOrder());
        ZonedDateTime hourIntervalStart = tollPasses.get(0);
        ZonedDateTime dayIntervalStart = tollPasses.get(0);
        List<ZonedDateTime> passesInHour = new ArrayList<>();
        int currHourFee = 0;
        int currHourFeeTotal = 0;
        List<HourInterval> passesInDay = new ArrayList<>();
        int currDayFee = 0;
        int currDayFeeTotal = 0;
        final int maxDailyFee = 60;
        final List<DayInterval> passesByDays = new ArrayList<>();
        int feeAtTime = 0;
        for (final ZonedDateTime tollPass : sortedTollPasses)
        {
            feeAtTime = getHourlyRate(vehicle.getType(), tollPass);
            if (!tollPass.isBefore(hourIntervalStart.plus(1, ChronoUnit.HOURS)))
            {
                // new hour interval
                hourIntervalStart = tollPass;
                // price for current day is never more than maxDailyFee
                currDayFee = Math.min(currDayFee + currHourFee, maxDailyFee);
                currDayFeeTotal += currHourFee;
                // make hour interval with fee and all passes
                passesInDay.add(new HourInterval(currHourFee, currHourFeeTotal, passesInHour));
                // reset
                passesInHour = new ArrayList<>();
                currHourFee = 0;
                currHourFeeTotal = 0;
                // new hour
                passesInHour.add(tollPass);
                currHourFee = Math.max(feeAtTime, currHourFee);
                currHourFeeTotal += feeAtTime;
                // is new day?
                if (!tollPass.isBefore(dayIntervalStart.plus(1, ChronoUnit.DAYS)))
                {
                    // new day interval
                    dayIntervalStart = tollPass;
                    // add previous day
                    passesByDays.add(new DayInterval(currDayFee, currDayFeeTotal, passesInDay));
                    // reset
                    passesInDay = new ArrayList<>();
                    currDayFee = 0;
                    currDayFeeTotal = 0;
                }
            }
            else
            {
                passesInHour.add(tollPass);
                currHourFee = Math.max(feeAtTime, currHourFee);
                currHourFeeTotal += feeAtTime;
            }
        }
        if (!passesInHour.isEmpty())
        {
            currDayFee = Math.min(currDayFee + currHourFee, maxDailyFee);
            currDayFeeTotal += currHourFee;

            passesInDay.add(new HourInterval(currHourFee, currHourFeeTotal, passesInHour));
            passesByDays.add(new DayInterval(currDayFee, currDayFeeTotal, passesInDay));
        }
        else if (!passesInDay.isEmpty())
        {
            passesByDays.add(new DayInterval(currDayFee, currDayFeeTotal, passesInDay));
        }
        return new ResultImpl(passesByDays, vehicle, ZonedDateTime.now());
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


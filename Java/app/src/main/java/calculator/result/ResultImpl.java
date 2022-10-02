package calculator.result;

import calculator.fees.DayFee;
import calculator.fees.DayInterval;
import calculator.fees.HourFee;
import calculator.fees.HourInterval;
import calculator.result.Result;
import calculator.vehicle.Vehicle;

import java.time.ZonedDateTime;
import java.util.List;
import java.util.Set;

public class ResultImpl implements Result
{
    private final List<DayFee> dayFees;
    private final Vehicle vehicle;
    private final ZonedDateTime creationTime;

    public ResultImpl(List<DayFee> dayFees, Vehicle vehicle, ZonedDateTime creationTime)
    {
        this.dayFees = dayFees;
        this.vehicle = vehicle;
        this.creationTime = creationTime;
    }

    public int getTotalCost()
    {
        return dayFees.stream()
                      .mapToInt(DayFee::getTotalFee)
                      .reduce(0, Integer::sum);
    }

    public int getActualFee()
    {
        return dayFees.stream()
                      .mapToInt(DayFee::getActualFee)
                      .reduce(0, Integer::sum);
    }

    @Override
    public int numberOfPasses()
    {
        return dayFees.stream()
                      .mapToInt(DayFee::getNumberOfPasses)
                      .reduce(0, Integer::sum);
    }

    public ZonedDateTime getCreationTime()
    {
        return creationTime;
    }

    // todo should be sorted
    @Override
    public ZonedDateTime getStartOfPeriod()
    {
        if (dayFees.isEmpty())
            throw new RuntimeException("List of DayFee dayFees is empty, can't calculate" + " result. ");
        return dayFees.get(0)
                      .getStart();
    }

    // todo should be sorted
    @Override
    public ZonedDateTime getEndOfPeriod()
    {
        if (dayFees.isEmpty())
            throw new RuntimeException("List of DayFee dayFees is empty, can't calculate" + " result. ");
        Set<HourFee> hours = dayFees.get(dayFees.size() - 1)
                                    .getHourFees();
        HourFee hourFee = hours.stream()
                               .reduce(null, (latest, next) ->
                               {
                                   if (latest == null) return next;
                                   return latest.getStart()
                                                .isAfter(next.getStart()) ? latest : next;
                               });
        return hourFee.getPasses()
                      .get(hourFee.getPasses()
                                  .size() - 1);
    }

    public String getVehicleId()
    {
        return vehicle.getRegistrationId();
    }

    public String getVehicleType()
    {
        return vehicle.getType()
                      .getType();
    }
}

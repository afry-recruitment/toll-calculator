package calculator.fees;

import calculator.vehicle.Vehicle;

import java.time.ZonedDateTime;
import java.util.List;

public class ResultImpl implements Result
{
    private final List<DayInterval> dayIntervals;
    private final Vehicle vehicle;
    private final ZonedDateTime creationTime;

    public ResultImpl(List<DayInterval> dayIntervals, Vehicle vehicle, ZonedDateTime creationTime)
    {
        this.dayIntervals = dayIntervals;
        this.vehicle = vehicle;
        this.creationTime = creationTime;
    }

    public int getTotalCost()
    {
        return dayIntervals.stream()
                           .mapToInt(DayInterval::totalFee)
                           .reduce(0, Integer::sum);
    }

    public int getActualFee()
    {
        return dayIntervals.stream()
                           .mapToInt(DayInterval::actualFee)
                           .reduce(0, Integer::sum);
    }

    @Override
    public int numberOfPasses()
    {
        return dayIntervals.stream()
                           .flatMap(di -> di.hourIntervals()
                                            .stream())
                           .mapToInt(it -> it.passes()
                                             .size())
                           .reduce(Integer::sum)
                           .orElse(0);
    }

    public ZonedDateTime getCreationTime()
    {
        return creationTime;
    }

    // todo should be sorted
    @Override
    public ZonedDateTime getStartOfPeriod()
    {
        if(dayIntervals.isEmpty()) return null;
        return dayIntervals.get(0)
                           .hourIntervals()
                           .get(0)
                           .passes()
                           .get(0);
    }

    // todo should be sorted
    @Override
    public ZonedDateTime getEndOfPeriod()
    {
        if(dayIntervals.isEmpty()) return null;
        List<HourInterval> hours = dayIntervals.get(dayIntervals.size() - 1)
                                               .hourIntervals();
        List<ZonedDateTime> passes = hours.get(hours.size() - 1)
                                          .passes();
        return passes.get(passes.size() - 1);

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

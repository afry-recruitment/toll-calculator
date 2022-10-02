package calculator.result;

import calculator.fees.DayFee;
import calculator.vehicle.Vehicle;
import java.time.ZonedDateTime;
import java.util.List;

public class ResultFactory
{
    private ResultFactory()
    {
    }

    public static Result getResult(List<DayFee> dayFees,
                                   Vehicle vehicle,
                                   ZonedDateTime creationTime)
    {
        return new ResultImpl(dayFees, vehicle, creationTime);
    }
}

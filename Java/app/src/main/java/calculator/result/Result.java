package calculator.result;

import java.time.ZonedDateTime;

public interface Result
{
    int getTotalCost();

    int getActualFee();

    int numberOfPasses();

    ZonedDateTime getCreationTime();

    ZonedDateTime getStartOfPeriod();

    ZonedDateTime getEndOfPeriod();

    String getVehicleId();

    String getVehicleType();
}

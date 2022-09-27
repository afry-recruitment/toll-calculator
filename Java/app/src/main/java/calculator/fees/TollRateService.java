package calculator.fees;

import java.time.LocalTime;

public interface TollRateService
{
    int getTollFeeAtTime(final LocalTime localTime);
}

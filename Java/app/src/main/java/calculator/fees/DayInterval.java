package calculator.fees;

import java.util.List;

public record DayInterval(int actualFee,int totalFee, List<HourInterval> hourIntervals)
{
}

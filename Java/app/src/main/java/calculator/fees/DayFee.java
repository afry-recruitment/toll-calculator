package calculator.fees;

import org.checkerframework.checker.units.qual.A;

import javax.annotation.Nonnull;
import java.time.ZonedDateTime;
import java.time.temporal.ChronoUnit;
import java.util.*;

public class DayFee
{
    private final ZonedDateTime start;
    private int actualFee;
    private int maxFee;
    private int totalFee;

    private Map<HourFee, Integer> passesCostMap;

    public DayFee(ZonedDateTime start, int maxFee)
    {
        this.start = start;
        this.maxFee = maxFee;
        this.actualFee = 0;
        this.totalFee = 0;
        this.passesCostMap = new HashMap<>();
    }

    public DayFee(HourFee hourFee, int maxFee)
    {
        this(hourFee.getStart(), maxFee);
        add(hourFee);
    }

    public void add(HourFee hourFee) // 0 <= hourActual AND hourActual <= hourActual.getLargestFee
    {
        this.totalFee += hourFee.getTotalFee();
        int newActualFee = this.actualFee + hourFee.getLargestFee();
        int lefUntilMaxFee = this.maxFee - newActualFee;
        int paidToReachMaxFee = hourFee.getLargestFee() - Math.abs(lefUntilMaxFee);
        // leftUntilMaxFee is at most maxFee + hourfee.largestFee
        int hourActualCost = lefUntilMaxFee >= 0 ? hourFee.getLargestFee() : paidToReachMaxFee;
        this.passesCostMap.put(hourFee, hourActualCost);
    }

    /**
     * @param passage timestamp of toll passage to check.
     * @return true if passage within one day of start or if within an hour of any HourFees in
     * passesCostMap keySet.
     */
    public boolean contains(ZonedDateTime passage)
    {
        return !passage.isBefore(start.plus(1, ChronoUnit.DAYS)) || containedHourFeesContains(passage);
    }

    /**
     * @param passage timestamp of toll passage to check.
     * @return true if any of the contained HourFees represents an interval encompassing the give passage.
     */
    private boolean containedHourFeesContains(@Nonnull ZonedDateTime passage)
    {
        return passesCostMap.keySet()
                            .stream()
                            .anyMatch(it -> it.contains(passage));
    }

    public boolean isEmpty()
    {
        return this.passesCostMap.keySet()
                                 .isEmpty();
    }

    public ZonedDateTime getStart()
    {
        return start;
    }

    public int getActualFee()
    {
        return actualFee;
    }

    public int getMaxFee()
    {
        return maxFee;
    }

    public int getTotalFee()
    {
        return totalFee;
    }

    public Set<HourFee> getHourFees()
    {
        return passesCostMap.keySet();
    }

    public int getHourFeeActualCost(HourFee hourFee)
    {
        return passesCostMap.get(hourFee);
    }

    public int getNumberOfPasses()
    {
        return getHourFees().stream()
                            .mapToInt(it -> it.getPasses()
                                              .size())
                            .reduce(0, Integer::sum);
    }
}

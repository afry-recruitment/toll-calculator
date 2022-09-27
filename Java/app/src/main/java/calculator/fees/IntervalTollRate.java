package calculator.fees;

import java.time.LocalTime;

/**
 * Represents a time window with a cost rate associated.
 */
public class IntervalTollRate
{
    private LocalTime start;
    private LocalTime end;
    private int rate;

    public IntervalTollRate(LocalTime start, LocalTime end, int rate)
    {
        this.start = start;
        this.end = end;
        this.rate = rate;
    }

    /**
     *
     * @param time test if time is after or equal to start and (inclusive) before end (exclusive)
     * @return true if in between
     */
    public boolean encompasses(LocalTime time)
    {
        return (!time.isBefore(getStart()) && time.isBefore(getEnd()));
    }

    public int getRate()
    {
        return this.rate;
    }

    public LocalTime getStart()
    {
        return start;
    }

    public LocalTime getEnd()
    {
        return end;
    }
}

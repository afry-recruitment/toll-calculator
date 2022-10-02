package calculator.fees;

import java.time.ZonedDateTime;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.List;

public class HourFee
{
    private final ZonedDateTime start;
    private ZonedDateTime highestFeePassage;
    private final List<ZonedDateTime> passes;
    private int largestFee;
    private int totalFee;

    public HourFee(ZonedDateTime start)
    {
        this.start = start;
        this.highestFeePassage = start;
        this.passes = new ArrayList<>();
        this.largestFee = 0;
        this.totalFee = 0;
    }

    public HourFee(ZonedDateTime start, int startFee)
    {
        this.start = start;
        this.highestFeePassage = start;
        this.passes = List.of(start);
        this.largestFee = startFee;
        this.totalFee = startFee;
    }

    public void addFee(ZonedDateTime passage, int fee)
    {
        this.totalFee += fee;
        if (this.largestFee < fee) this.highestFeePassage = passage;
        this.largestFee = Math.max(this.largestFee, fee);
        this.passes.add(passage);
    }

   public boolean contains(ZonedDateTime passage)
    {
        return !passage.isBefore(start.plus(1, ChronoUnit.HOURS));
    }

    public int getLargestFee()
    {
        return largestFee;
    }

    public int getTotalFee()
    {
        return totalFee;
    }
    public boolean isEmpty(){
        return this.passes.isEmpty();
    }

    public ZonedDateTime getStart()
    {
        return start;
    }

    public ZonedDateTime getHighestFeePassage()
    {
        return highestFeePassage;
    }

    public List<ZonedDateTime> getPasses()
    {
        return new ArrayList<>(passes);
    }
}
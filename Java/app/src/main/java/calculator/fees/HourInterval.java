package calculator.fees;

import java.time.ZonedDateTime;
import java.util.List;

public record HourInterval(int actualFee, int totalFee, List<ZonedDateTime> passes)
{
    @Override
    public String toString()
    {
        return "Actual fee: " + this.actualFee() + System.lineSeparator() + "Total fee: " + this.totalFee +
               System.lineSeparator() + "Money saved: " + (this.totalFee - this.actualFee) +
               System.lineSeparator() + "Passages: " + System.lineSeparator() + makePassesString();
    }

    private String makePassesString()
    {
        StringBuilder sum = new StringBuilder();
        for (ZonedDateTime dateTime : this.passes)
        {
            sum.append(dateTime)
               .append(System.lineSeparator());
        }
        return sum.toString();
    }
}

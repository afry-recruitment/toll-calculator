using System.Collections;

namespace TollFeeCalculator.Test;

public class AllDays : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        const int daysIn2013 = 365;
        for (int i = 0; i < daysIn2013; i++)
        {
            int dayOfYear = i + 1;
            yield return new object[] { dayOfYear };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

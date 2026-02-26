using TollFeeCalculator.Models;

namespace TollFeeCalculator.Factories;

public static class TollFeeCalculatorConfigFactory
{
    public static TollFeeCalculatorConfig CreateDefault()
    {
        return new TollFeeCalculatorConfig
        {
            Currency = "SEK",
            DailyMaxFee = 60,
            ChargeIntervalMinutes = 60,
            FeeSchedule = new List<TollFeeInterval>
            {
                // 8 SEK
                new(
                    Start: new TimeOnly(6, 0), 
                    End: new TimeOnly(6, 30), 
                    Fee: 8),

                new(
                    Start: new TimeOnly(8, 30), 
                    End: new TimeOnly(15, 0), 
                    Fee: 8),

                new(
                    Start: new TimeOnly(18, 0), 
                    End: new TimeOnly(18, 30), 
                    Fee: 8),

                // 13 SEK
                new(
                    Start: new TimeOnly(6, 30), 
                    End: new TimeOnly(7, 0), 
                    Fee: 13),

                new(
                    Start: new TimeOnly(8, 0), 
                    End: new TimeOnly(8, 30), 
                    Fee: 13),

                new(
                    Start: new TimeOnly(15, 0), 
                    End: new TimeOnly(15, 30), 
                    Fee: 13),

                new(
                    Start: new TimeOnly(17, 0), 
                    End: new TimeOnly(18, 0), 
                    Fee: 13),

                // 18 SEK
                new(
                    Start: new TimeOnly(7, 0), 
                    End: new TimeOnly(8, 0), 
                    Fee: 18),

                new(
                    Start: new TimeOnly(15, 30), 
                    End: new TimeOnly(17, 0), 
                    Fee: 18)
            }
        };
    }
}
namespace CityTollFeeCalculator.Tolls;

internal static class TollCalculatorExtentions
{
    public static bool IsWithinOneHour(this DateTime previousTime, DateTime currentTime)
    {
        var timeSinceLastToll = currentTime - previousTime;
        return previousTime.Date == currentTime.Date && timeSinceLastToll < TimeSpan.FromMinutes(60);

    }
}

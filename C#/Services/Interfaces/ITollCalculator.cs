using TollFeeCalculator.Models;

namespace TollFeeCalculator.Services.Interfaces;

public interface ITollCalculator
{
    void UpdateDailyToll(Vehicle vehicle);
    int GetTollFee(TimeSpan currentTime);
}

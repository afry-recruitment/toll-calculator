using RoadToll.Application.Models;

namespace RoadToll.Application.Calculator
{
    public interface ITollCalculator
    {
        int GetTollFee(Vehicle vehicle, DateTime[] dates);
    }
}

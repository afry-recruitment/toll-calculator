using System;
namespace TollFeeCalculator
{
    public interface ITollCalculator
    {
        public int GetTotalTollFee(IVehicle vehicle, DateOnly date, TimeOnly[] times);
        public int GetTollFee(DateTime date, IVehicle vehicle);
    }
}
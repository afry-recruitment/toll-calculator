using System;
namespace TollFeeCalculator
{
    public interface ITollCalculator
    {
        public int GetTotalTollFee(IVehicle vehicle, DateTime[] dates);
        public int GetTollFee(DateTime date, IVehicle vehicle);
    }
}
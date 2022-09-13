using System;
namespace TollFeeCalculator
{
    public interface ITollCalculator
    {
        public int GetTollFee(IVehicle vehicle, DateTime[] dates);
        public int GetTollFee(DateTime date, IVehicle vehicle);
    }
}
using System;

namespace Test_C__app.Afry_toll_calculator_fork.C_
{
    public interface TollCalculator
    {
        int GetTollFee(VehicleType vehicle, DateTime[] dates);
        int GetTollFeeForDay(VehicleType vehicle, DateTime[] dayTimeStamps);
        int GetTollFee(VehicleType vehicle, DateTime date);
        VehicleType[] TollFreeVehicleTypes { get; }
    }
    public enum VehicleType
    {
        Car = 0,
        Motorbike = 1,
        Tractor = 2,
        Emergency = 3,
        Diplomat = 4,
        Foreign = 5,
        Military = 6
    }
}

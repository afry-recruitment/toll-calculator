using TollFeeCalculator.Enterprise.Attributes;

namespace TollFeeCalculator.Enterprise.Enums
{
    public enum VehicleType
    {
        Car = 0,

        [TollFree]
        Motorbike = 1,

        [TollFree] 
        Tractor = 2,

        [TollFree] 
        Emergency = 3,

        [TollFree] 
        Diplomat = 4,

        [TollFree] 
        Foreign = 5,

        [TollFree] 
        Military = 6
    }
}

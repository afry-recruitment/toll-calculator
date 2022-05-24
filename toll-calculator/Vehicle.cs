namespace TollFeeCalculator
{
    public enum VehicleTypeEnum
    {
        Car = 0,
        Motorbike = 1,
        Tractor = 2,
        Emergency = 3,
        Diplomat = 4,
        Foreign = 5,
        Military = 6
    }

    public class Vehicle
    {
        public Vehicle(VehicleTypeEnum vehicleType)
        {
            VehicleType = vehicleType;
        }

        public VehicleTypeEnum VehicleType { get; set; }

        public bool IsTollFreeVechicle()
        {
            switch (VehicleType)
            {
                case VehicleTypeEnum.Motorbike:
                case VehicleTypeEnum.Tractor:
                case VehicleTypeEnum.Emergency:
                case VehicleTypeEnum.Diplomat:
                case VehicleTypeEnum.Foreign:
                case VehicleTypeEnum.Military:
                    return true;
            }

            return false;
        }
    }
}

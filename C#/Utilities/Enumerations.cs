namespace TollFeeCalculator.Utilities
{ 
    public enum VehicleType
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5,
        Personal = 6,
        Other = 7
    }
    public enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }

    public enum OwnerType
    {
        Private,
        Company,
        Government,
        Other
    }

    // Decided to go with levels instead of Low, Medium, High in case more fees get added.
    public enum TollFee
    {
        FeeLevel1 = 8,
        FeeLevel2 = 13,
        FeeLevel3 = 18,
    }
}
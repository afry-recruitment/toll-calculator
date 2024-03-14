namespace TollFeeCalculator.Models
{
    // I was a little bit unsure about this refactoring, but it seamed legit in my eyes. I suppose a vehicle could be a Car and a Diplomat,
    // but in terms of the tolls this makes no difference. Still a bit unsure about all of this though. 
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

public class Car : IVehicle
{
    // Ett designsätt skulle kunna vara att jag haft isTollFreeVehicle som ett attribut redan här.
    public VehicleType Type => VehicleType.Car;
}
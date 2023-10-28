public enum VehicleType { Car, Truck, Bike, Trike, UFO, Motorbike, Tractor, Emergency, Diplomat, Foreign, Military, Unidentified }

public class Vehicle
{
    public string licensePlate = "Undefined";
    public VehicleType vehicleType = VehicleType.Car;

    public Vehicle(string plate, VehicleType type) 
    {
        licensePlate = plate;
        vehicleType = type;
    }
}

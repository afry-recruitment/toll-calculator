namespace TollFeeCalculator.Models
{
    // I saw no real point in the previous use of car and motorcycle and inherting from a interface, so I simplified it.
    // Added a bool-flag to be able to quickly check if the vehicle is free or not. 
    public class Vehicle
    {
        public bool IsFreeVehicle { get; set; } = true;
        public Vehicle(VehicleType vehicleType)
        {
            if (vehicleType is not VehicleType.Car)
                IsFreeVehicle = false;
        }
        // If we in the future would like to add other payment-rules, this could easily be added or modified in the ctor and using 
        // the IsFreeVehicle-property.
    }
}
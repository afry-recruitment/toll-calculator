namespace TollFeeCalculator
{
    public interface IVehicle
    {
        // String RegistrationNumber();  // Bör kanske hämta ett registreringsnummer så man kan göra varje bil unik i en databas?
        string VehicleType { get; }
        bool IsTollFreeVehicle { get; set; }
    }
}
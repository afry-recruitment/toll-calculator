package calculator.vehicle;

public class Vehicle
{
    private VehicleType type;
    private String registrationId;

    public Vehicle(VehicleType type, String registrationId)
    {
        this.type = type;
        this.registrationId = registrationId;
    }

    public VehicleType getType()
    {
        return type;
    }

    public String getRegistrationId()
    {
        return registrationId;
    }
}

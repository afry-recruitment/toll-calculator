namespace TollCalculator.Tests.UnitTests;

public class VehicleTests
{
    [TestCase("Motorbike", true)]
    [TestCase("Tractor", true)]
    [TestCase("EmergencyVehicle", true)]
    [TestCase("DiplomaticVehicle", true)]
    [TestCase("ForeignVehicle", true)]
    [TestCase("MilitaryVehicle", true)]
    [TestCase("Car", false)]
    public void IsTollFreeVehicle_Returns_Expected_ForVehicleType(string vehicleType, bool expected)
    {
        var vehicle = VehicleFactory.Create(vehicleType);

        Assert.That(vehicle.IsTollFree(), Is.EqualTo(expected));
    }
    
    [TestCase("Motorbike")]
    [TestCase("Tractor")]
    [TestCase("EmergencyVehicle")]
    [TestCase("DiplomaticVehicle")]
    [TestCase("ForeignVehicle")]
    [TestCase("MilitaryVehicle")]
    [TestCase("Car")]
    public void GetVehicleType_Returns_Expected_VehicleType(string expected)
    {
        var vehicle = VehicleFactory.Create(expected);

        Assert.That(vehicle.GetVehicleType(), Is.EqualTo(expected));
    }
}
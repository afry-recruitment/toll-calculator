namespace TollFeeCalculator;

public class Car : IVehicle
{
    public bool IsTollFree() { return false; }
}
public class Motorbike : IVehicle
{ 
    public bool IsTollFree() { return true; }
}
public class Tractor : IVehicle
{
    public bool IsTollFree() { return true; }
}
public class Emergency : IVehicle
{
    public bool IsTollFree() { return true; }
}
public class Diplomat : IVehicle
{
    public bool IsTollFree() { return true; }
}
public class Foreign : IVehicle
{
    public bool IsTollFree() { return true; }
}
public class Military : IVehicle
{
    public bool IsTollFree() { return true; }
}  

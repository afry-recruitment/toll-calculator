namespace TollFeeCalculator.Vehicle
{
  public class VehicleService
  {
    public int VehicleId(Vehicle vehicle)
    {
      TollFreeVehicles candidate;
      if (Enum.TryParse<TollFreeVehicles>(vehicle.GetVehicleType(), out candidate))
      {
        return (int)candidate;
      }
      else
      {
        return 6;
      }
    }
    private enum TollFreeVehicles
    {
      Motorbike = 0,
      Tractor = 1,
      Emergency = 2,
      Diplomat = 3,
      Foreign = 4,
      Military = 5
    }
  }
}
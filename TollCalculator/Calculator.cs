
using TollFeeCalculator.Vehicle;
namespace TollFeeCalculator
{
  public class Calculator
  {
    private const int NotTollFree = 6;
    private IFeeService feeService;
    private ITollFreeDateService tollFreeDateService;
    private bool isTollFreeVehicle(int vehicleType) => vehicleType < NotTollFree;

    public Calculator(IFeeService feeService, ITollFreeDateService tollFreeDateService)
    {
      this.feeService = feeService;
      this.tollFreeDateService = tollFreeDateService;
    }

    // old interface, in case we need to support it. 
    [Obsolete("call with vehicle is deprecated, use int for vehicletype instead.")]
    public int TollFee(Vehicle.Vehicle vehicle, DateTime[] dates)
    {
      // Guard
      if (vehicle == null)
        throw new ArgumentOutOfRangeException("Vehicle is null. function is depricated, use subscript with vehicletype instead)");

      return TollFee(new VehicleService().VehicleId(vehicle), dates);
    }

    // new interface - vehicle does not really do anything, provided that we have a contract
    // that states the vehicletypes, the vehicleType is all we need. 
    public int TollFee(int vehicleType, DateTime[] dates)
    {
      if (!GuardsAndMoreThanZero(vehicleType, dates))
        return 0;

      // assume dates can be unordered 
      var datelist = dates.ToList<DateTime>().Order();
      int totalFee = 0;
      int currentFee = 0;

      DateTime currentDateTime = dates[0];

      foreach (DateTime date in datelist)
      {
        int nextFee = feeService.FeeForTime(TimeOnly.FromDateTime(date));
        TimeSpan dif = date - currentDateTime;
        if (dif.TotalHours < 1)
        {
          if (currentFee < nextFee)
          {
            currentFee = nextFee;
          }
        }
        else
        {
          totalFee += currentFee;
          currentFee = nextFee;
        }
        currentDateTime = date;
      }
      totalFee += currentFee;

      return totalFee < 60 ? totalFee : 60;
    }
    private bool GuardsAndMoreThanZero(int vehicleType, DateTime[] dates)
    {

      if (vehicleType < 0)
        throw new ArgumentOutOfRangeException("vehicleType must be > -1");

      // According to spec, all dates passed in occur on the same day, throw error if several dates
      if (dates.Any(date => date.Day != dates[0].Day || date.Month != dates[0].Month || date.Year != dates[0].Year))
      {
        throw new ArgumentOutOfRangeException("Dates passed to calculation are not the same (all dates passed should be from same day)");
      }

      if (isTollFreeVehicle(vehicleType) || dates.Length == 0)
        return false;

      if (tollFreeDateService.IsTollFree(dates[0]))
        return false;

      return true;
    }
  }
}

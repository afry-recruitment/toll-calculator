using System.Data;
using System.Globalization;
using System.Reflection.Metadata;

namespace TollFeeCalculator
{
  public class Calculator
  {
    public const uint NotTollFree = 6;
    private IFeeService feeService;
    private ITollFreeDateService tollFreeDateService;
    private bool isTollFreeVehicle(uint vehicleType) => vehicleType < NotTollFree;

    public Calculator(IFeeService feeService, ITollFreeDateService tollFreeDateService)
    {
      this.feeService = feeService;
      this.tollFreeDateService = tollFreeDateService;
    }

    public int TollFee(uint vehicleType, DateTime[] dates)
    {
      // Guards
      if (isTollFreeVehicle(vehicleType) || dates.Length == 0)
        return 0;

      // According to spec, all dates passed in occur on the same day, throw error if false
      if (dates.Any(date => date.Day != dates[0].Day || date.Month != dates[0].Month || date.Year == dates[0].Year))
      {
        throw new ArgumentOutOfRangeException("Dates passed to calculation are not the same (all dates passed should be from same day)");
      }

      if (tollFreeDateService.IsTollFree(dates[0]))
      {
        return 0;
      }

      // assume dates can be unordered 
      var datelist = dates.ToList<DateTime>().Order();

      int totalFee = 0;
      int currentFee = 0;
      int currentMillie = dates[0].Millisecond;

      foreach (DateTime date in datelist)
      {
        int nextFee = feeService.FeeForTime(TimeOnly.FromDateTime(date));
        int minutes = (date.Millisecond - currentMillie) / 1000 / 60;
        if (minutes <= 60)
        {
          if (currentFee < nextFee)
          {
            currentFee = nextFee;
          }
        }
        else
        {
          totalFee += currentFee;
          currentFee = 0;
        }
        currentMillie = date.Millisecond;
      }
      totalFee += currentFee;

      return totalFee < 60 ? totalFee : 60;

    }
  }
}

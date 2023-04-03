namespace TollCalculatorSana.Services;

public class FeeService : IFeeService
{
    private List<FeeInterval> feeIntervals;
    private List<TollFreeVehicleType> tollFreeVehicleTypes;
    private List<FreeDate> freeDates;
    private List<RegulationValues> regulations;

    private readonly IJSONService _jSONService;

    public FeeService(IJSONService jSONService)
    {
        _jSONService = jSONService;

        // Getting values once
        InitializeLists();
    }

    public int GetTollFee(string vehicleRegNum, string vehicleType, List<DateTime> dates)
    {

        if (string.IsNullOrEmpty(vehicleRegNum)) { throw new Exception("No reg number."); }

        if (dates == null || dates.Count == 0) { throw new Exception("No dates."); }


        if (string.IsNullOrEmpty(vehicleType)) { throw new Exception("No vehicle type."); }

        dates = dates.OrderBy(i => i).ToList();
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicleRegNum, vehicleType);
            int tempFee = GetTollFee(intervalStart, vehicleRegNum, vehicleType);

            // Correction: the calculation of difference between dates
            TimeSpan diffInMillies = date - intervalStart;
            double minutes = diffInMillies.TotalMilliseconds / 1000 / 60;

            if (minutes <= regulations.Find(x => x.Name.Equals("MaxPeriod")).Value)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
                // Correction: holding the current date as the start for the next round
                intervalStart = date;
            }
        }
        if (totalFee > regulations.Find(x => x.Name.Equals("MaxFee")).Value) totalFee = regulations.Find(x => x.Name.Equals("MaxFee")).Value;
        return totalFee;
    }

    private void InitializeLists()
    {
        // Getting the fee intervals/values stored in JSON file (could be fetched from an external API)
        feeIntervals = _jSONService.GetJSON<FeeInterval>("./LookupData/Fees.json");

        // Getting the vehicle types and if they are tollfree stored in JSON file (could be fetched from an external API for the specific type only)
        tollFreeVehicleTypes = _jSONService.GetJSON<TollFreeVehicleType>("./LookupData/TollFreeVehicleTypes.json");

        // Getting the dates which are tollfree stored in JSON file (could be fetched from an external API)
        freeDates = _jSONService.GetJSON<FreeDate>("./LookupData/TollFreeDates.json");

        // Getting the dates which are tollfree stored in JSON file (could be fetched from an external API)
        regulations = _jSONService.GetJSON<RegulationValues>("./LookupData/RegulationValues.json");
    }

    private int GetTollFee(DateTime date, string vehicleRegNum, string vehicleType)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicleType)) return 0;

        if (feeIntervals.Count != 0)
        {
            // Getting the matching fee interval to the current datetime
            var item = feeIntervals.FirstOrDefault(fi => TimeOnly.FromDateTime(date) >= fi.From && TimeOnly.FromDateTime(date) <= fi.To);
            return item != null ? item.FeeValue : 0;
        }

        return 0;
    }
    
    private bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        // Checking if the current date is tollfree (could be fetched from an external API or DB for the specific date)
        var item = freeDates.FirstOrDefault(fd => fd.Date.Equals(date.Date));

        return item != null;
    }

    private bool IsTollFreeVehicle(string vehicleType)
    {
        if (vehicleType.Equals(""))
            return false;

        if (tollFreeVehicleTypes.Count != 0)
        {
            // Checking if the current vehicle is tollfree (could be fetched from an external API or DB for the specific vehicle)
            var item = tollFreeVehicleTypes.FirstOrDefault(v => v.Type.ToLower().Equals(vehicleType.ToLower()));
            return item != null;
        }

        return false;
    }
}

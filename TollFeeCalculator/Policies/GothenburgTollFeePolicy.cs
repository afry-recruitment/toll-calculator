using PublicHoliday;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Policies;

public class GothenburgTollFeePolicy : ITollFeePolicy
{
    public string Currency => "SEK";
    public decimal DailyMaxFee => 60;
    public TollFeeRate[] TollFeeRates =>
    [
        new(new TimeOnly(6, 0), new TimeOnly(6, 29), LowFee), // 06:00 - 06:29
        new(new TimeOnly(6, 30), new TimeOnly(6, 59), MediumFee), // 06:30 - 06:59
        new(new TimeOnly(7, 0), new TimeOnly(7, 59), HighFee), // 07:00 - 07:59
        new(new TimeOnly(8, 0), new TimeOnly(8, 29), MediumFee), // 08:00 - 08:29
        new(new TimeOnly(8, 30), new TimeOnly(14, 59), LowFee), // 08:30 - 14:59
        new(new TimeOnly(15, 0), new TimeOnly(15, 29), MediumFee), // 15:00 - 15:29
        new(new TimeOnly(15, 30), new TimeOnly(16, 59), HighFee), // 15:30 - 16:59
        new(new TimeOnly(17, 0), new TimeOnly(17, 59), MediumFee), // 17:00 - 17:59
        new(new TimeOnly(18, 0), new TimeOnly(18, 29), LowFee), // 18:00 - 18:29
        new(new TimeOnly(18, 30), new TimeOnly(5, 59), NoFee), // 18:30 - 05:59
    ];

    private const decimal LowFee = 8;
    private const decimal MediumFee = 13;
    private const decimal HighFee = 18;
    private const decimal NoFee = 0;

    private static readonly SwedenPublicHoliday HolidayService = new();

    public bool IsTollFreeDate(DateOnly date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        return HolidayService.IsPublicHoliday(new DateTime(date.Year, date.Month, date.Day));
    }

    public bool IsTollFreeVehicleType(VehicleType vehicleType) => vehicleType != VehicleType.Car;
}
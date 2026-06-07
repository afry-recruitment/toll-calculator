using TollCalculator.Vehicles;

namespace TollCalculator.Config
{
    public record TollCalculatorConfig(
        HashSet<VehicleType>? TollFreeVehicles,
        HashSet<string>? Holidays,
        Dictionary<FeeTimeZone, decimal>? TimeZoneFees,
        decimal? DailyFeeLimit);

    public record FeeTimeZone(TimeSpan Start, TimeSpan End);

    public static class TollCalculatorConfigDefaults
    {

        public static readonly HashSet<VehicleType> TollFreeVehicles = [
            VehicleType.Military,
            VehicleType.Emergency,
            VehicleType.Diplomat,
            VehicleType.Foreign,
        ];

        // MM-dd
        public static readonly HashSet<string> Holidays = [
            "12-24",
            "12-25",
            "01-01",
            // ...
        ];

        public static readonly Dictionary<FeeTimeZone, decimal> TimeZoneFees = new()
        {
            { new (Start: new( 6,  0,  0), End: new( 6, 29, 59)),  8 },
            { new (Start: new( 6, 30,  0), End: new( 6, 59, 59)), 13 },
            { new (Start: new( 7,  0,  0), End: new( 7, 59, 59)), 18 },
            { new (Start: new( 8,  0,  0), End: new( 8, 29, 59)), 13 },
            { new (Start: new( 8, 30,  0), End: new( 8, 59, 59)), 13 },
            { new (Start: new( 9,  0,  0), End: new(13, 59, 59)), 13 },
            { new (Start: new(14,  0,  0), End: new(16, 59, 59)), 18 },
            // ...
        };

        public static readonly decimal DailyFeeLimit = 60;        
    }
}

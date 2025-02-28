using TollFeeCalculator.Enums;

namespace TollFeeCalculator.Models.Configuration
{
    internal class TollFreeSettings
    {
        public string? Months { get; set; }
        public string? DayOfWeek { get; set; }
        public string? Dates { get; set; }
        public string? VehicleTypes { get; set; }

        public IEnumerable<VehicleTypes>? TollFreeVehicles { get
            {
                if(!string.IsNullOrWhiteSpace(VehicleTypes))
                {
                    var vehicles = VehicleTypes.Split(',');
                    return vehicles.Select(v => Enum.Parse<VehicleTypes>(v.Trim()));
                }

                return null;
            } 
        }

        public IEnumerable<int>? TollFreeMonths
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Months))
                {
                    var months = Months.Split(',');
                    return months.Select(m => DateTime.Parse($"1/{m}/{DateTime.Now.Year}").Month);
                }

                return null;
            }
        }

        public IEnumerable<DayOfWeek>? TollFreeWeekDays
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DayOfWeek))
                {
                    var dayOfWeek = DayOfWeek.Split(',');
                    return dayOfWeek.Select(d => Enum.Parse<DayOfWeek>(d.Trim()));
                }

                return null;
            }
        }

        public IEnumerable<DateTime>? GetTollFreeDates(DateTime now)
        {
            if (!string.IsNullOrWhiteSpace(Dates))
            {
                var dates = Dates.Split(',');
                return dates.Select(d => DateTime.ParseExact($"{d.Trim()}/{now.Year}", "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")));
            }

            return null;
        }
    }
}
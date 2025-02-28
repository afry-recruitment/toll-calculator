using Microsoft.Extensions.Options;
using TollFeeCalculator.Extensions;
using TollFeeCalculator.Models;
using TollFeeCalculator.Models.Configuration;
using TollFeeCalculator.Services.HolidayService;

namespace TollFeeCalculator.Services
{
    internal class TollService : ITollService
    {
        private readonly ILogger<TollService> _logger;
        private readonly IHolidayService _holidayService;
        private readonly TollSettings _tollSettings;
        private readonly IEnumerable<TollHour>? _feeHours;
        private readonly int _maxFeeForADay;

        public TollService(ILogger<TollService> logger, IHolidayService holidayService, IOptions<TollSettings> options)
        {
            _logger = logger;
            _holidayService = holidayService;
            _tollSettings = options.Value;
            _feeHours = _tollSettings.Fees?.SelectMany(f => f.Value);
            _maxFeeForADay = _tollSettings.MaxFee ?? 60;
        }
        
        /// <inheritdoc />
        public bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (_tollSettings.TollFree != null)
            {
                if (_tollSettings.TollFree.TollFreeVehicles?.Any(v => v.Equals(vehicle.VehicleType)) ?? false)
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <inheritdoc />
        public async Task<bool> IsTollFree(IVehicle vehicle, DateTime date)
        {
            try
            {

                if(_tollSettings.TollFree != null)
                {
                    if (IsTollFreeVehicle(vehicle))
                    {
                        return true;
                    }
                    else if(_tollSettings.TollFree.TollFreeMonths?.Any(m => m.Equals(date.Month)) ?? false)
                    {
                        return true;
                    }
                    else if (_tollSettings.TollFree.TollFreeWeekDays?.Any(m => m.Equals(date.DayOfWeek)) ?? false)
                    {
                        return true;
                    }
                    else if (_tollSettings.TollFree.GetTollFreeDates(date)?.Any(m => m.Equals(date)) ?? false)
                    {
                        return true;
                    }
                }

                return await _holidayService.IsHoliday(date);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while determining if vehicle is toll free");
            }

            return false;
        }
        
        /// <inheritdoc />
        public async Task<int> GetTollFee(IVehicle vehicle, DateTime date)
        {
            try
            {
                var isTollFree = await IsTollFree(vehicle, date);
                if(isTollFree)
                {
                    return 0;
                }

                if(_feeHours?.Any() ?? false)
                {
                    foreach(var feeHour in _feeHours)
                    {
                        if (date.TimeOfDay >= feeHour.StartTime && date.TimeOfDay < feeHour.EndTime)
                        {
                            return feeHour.Fee;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetTollFee)}: Error while calculating toll fee");
            }

            return 0;
        }
        
        /// <inheritdoc />
        public int GetTollFee(IVehicle vehicle, IEnumerable<DateTime> passageTimes)
        {
            try
            {
                if(IsTollFreeVehicle(vehicle) || !passageTimes.Any())
                {
                    return 0;
                }

                var days = passageTimes.OrderBy(d => d).GroupBy(d => new { d.Date});
                var totalFee = 0;

                for(int i = 0; i < days.Count(); i++)
                {
                    var entries = days.ElementAt(i);
                    var feeForDay = 0;
                    var entriesByEveryHour = entries.GroupWhile((groupDate, current) => (current - groupDate).TotalMinutes <= 60);

                    foreach(var hour in entriesByEveryHour)
                    {
                        var fees = hour.Select(async d => await GetTollFee(vehicle, d)).Select(t => t.Result);
                        feeForDay += fees.Max();
                    }

                    totalFee += Math.Min(feeForDay, _maxFeeForADay);
                }

                return totalFee;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetTollFee)}: Error while calculating total fee");
            }

            return 0;
        }
    }
}

using Microsoft.Extensions.Options;
using TollFeeCalculator.Application.Extensions;
using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Application.Models;
using TollFeeCalculator.Application.Options;
using TollFeeCalculator.Enterprise.Entities;
using TollFeeCalculator.Enterprise.Interfaces;

namespace TollFeeCalculator.Application.Services
{
    public sealed class TollFeeService : ITollFeeService
    {
        private readonly IFeeSchedule _feeSchedule;
        private readonly ICalendarService _calendarService;
        private readonly IVehicleService _vehicleService;
        private readonly TollFeeOptions _options;

        public TollFeeService(IFeeSchedule feeSchedule, ICalendarService calendarService, IVehicleService vehicleService, IOptions<TollFeeOptions> options) 
        {
            _feeSchedule = feeSchedule ?? throw new ArgumentNullException(nameof(feeSchedule));
            _calendarService = calendarService ?? throw new ArgumentNullException(nameof(calendarService));
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<GetTollFeeResultModel> GetFee(IReadOnlyList<DateTime> dateTimes, Vehicle vehicle, CancellationToken ct)
        {
            if (vehicle is null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }

            if (_vehicleService.IsTollFree(vehicle))
            {
                return new GetTollFeeResultModel(0);
            }

            if (dateTimes is null)
            {
                return new GetTollFeeResultModel(0);
            }

            var ordered = dateTimes.OrderBy(d => d).ToList();
            if (!ordered.Any())
            {
                return new GetTollFeeResultModel(0);
            }

            var total = 0;

            foreach (var dayGroup in ordered.GroupBy(d => DateOnly.FromDateTime(d)))
            {
                ct.ThrowIfCancellationRequested();

                var date = dayGroup.Key;

                if (await _calendarService.IsTollFreeAsync(date, ct).ConfigureAwait(false))
                {
                    continue;
                }

                total += CalculateDayTotal(dayGroup);
            }

            return new GetTollFeeResultModel(total);
        }

        private int CalculateDayTotal(IEnumerable<DateTime> passages)
        {
            var ordered = passages.OrderBy(p => p).Select(p => TimeOnly.FromDateTime(p)).ToList();
            if (!ordered.Any())
            {
                return 0;
            }

            var intervalStart = ordered.First();
            var dayTotal = 0;

            foreach (var chunk in ordered.ChunkByChargeWindow(_options.ChargeWindowMinutes))
            {
                var chunkFee = 0;

                foreach (var c in chunk)
                {
                    var fee = _feeSchedule.GetFee(c);
                    if (fee > chunkFee)
                    {
                        chunkFee = fee;
                    }
                }

                dayTotal += chunkFee;

                if (dayTotal > _options.MaxDailyFee)
                {
                    return _options.MaxDailyFee;
                }
            }
            
            return dayTotal;
        }
    }
}

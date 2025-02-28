using Microsoft.Extensions.Caching.Memory;
using TollFeeCalculator.Extensions;

namespace TollFeeCalculator.Services.HolidayService
{
    internal class HolidayService : IHolidayService
    {
        private readonly ILogger<HolidayService> _logger;
        private readonly IHolidayClient _holidayClient;
        private readonly IMemoryCache _memoryCache;
        private readonly string _languageCode;
        private readonly string _countryCode;

        public HolidayService(ILogger<HolidayService> logger, IHolidayClient holidayClient, IMemoryCache cache, IConfiguration configuration)
        {
            _logger = logger;
            _holidayClient = holidayClient;
            _memoryCache = cache;
            _countryCode = configuration.GetValue<string>("Country") ?? "";
            _languageCode = configuration.GetValue<string>("Language") ?? "";
        }

        /// <inheritdoc />
        public async Task<bool> IsHoliday(DateTime date)
        {
            var holidays = await GetActiveHolidaysForYear(date);
            if(holidays?.Any() ?? false)
            {
                if(holidays.Any(d => date.IsInbetween(d.StartDate, d.EndDate)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Fetches active national holidays for current year from cache or api <see cref="IHolidayClient"/>
        /// </summary>
        private async Task<IEnumerable<Models.Holiday>?> GetActiveHolidaysForYear(DateTime date)
        {
            try { 
                var currentYear = date.Year;
                var startDate = DateTime.Parse($"{currentYear}-01-01");
                var endDate = DateTime.Parse($"{currentYear + 1}-01-01");

                var cacheKey = $"{_countryCode}-{_languageCode}-{startDate}-{endDate}";
                IEnumerable<Models.Holiday>? holidays = null;

                if (!_memoryCache.TryGetValue(cacheKey, out holidays))
                {
                    holidays =  await _holidayClient.FetchHolidays(_countryCode, _languageCode, startDate, endDate);
                    if(holidays?.Any() ?? false)
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(2));

                        _memoryCache.Set(cacheKey, holidays, cacheEntryOptions);
                    }
                }

                return holidays;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching active holidays from cache or {nameof(IHolidayClient)}");
            }

            return null;
        }
    }
}

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Application.Options;

namespace TollFeeCalculator.Application.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IHolidayProvider _holidayProvider;
        private readonly TollFeeOptions _options;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CalendarService> _logger;

        public CalendarService(IHolidayProvider holidayProvider, IOptionsSnapshot<TollFeeOptions> options, IMemoryCache memoryCache, ILogger<CalendarService> logger)
        {
            _holidayProvider = holidayProvider ?? throw new ArgumentNullException(nameof(holidayProvider));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> IsTollFreeAsync(DateOnly date, CancellationToken ct)
        {
            if (_options.TollFreeWeekends && (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday))
            {
                return true;
            }

            var holidays = await GetHolidaysWithCachingAsync(date, ct).ConfigureAwait(false);

            if (!holidays.Any())
            {
                return _options.HolidaysFailResultsTollFree;
            }

            return holidays.Contains(date);
        }

        private string GetCacheKey(int year) => $"holidays_{year}";

        private async Task<IReadOnlySet<DateOnly>> GetHolidaysWithCachingAsync(DateOnly date, CancellationToken ct)
        {
            var cacheKey = GetCacheKey(date.Year);
            var cacheDuration = TimeSpan.FromHours(_options.HolidaysCacheDurationHours);

            if (_cache.TryGetValue<IReadOnlySet<DateOnly>>(cacheKey, out var cached) && cached != null)
            {
                return cached;
            }

            ct.ThrowIfCancellationRequested();

            IReadOnlySet<DateOnly> holidays = new HashSet<DateOnly>();
            try
            {
                holidays = await _holidayProvider.GetHolidaysAsync(date.Year, ct).ConfigureAwait(false) ?? new HashSet<DateOnly>();

                if (!holidays.Any())
                {
                    _logger.LogError("Holiday provider returned empty set for year {Year}.", date.Year);
                    cacheDuration = TimeSpan.FromMinutes(5);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load holidays for year {Year}. Using empty set.", date.Year);
                cacheDuration = TimeSpan.FromMinutes(5);
                holidays = new HashSet<DateOnly>();
            }

            _cache.Set(cacheKey, holidays, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            });

            return holidays;
        }
    }
}

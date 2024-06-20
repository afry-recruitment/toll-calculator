using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Globalization;
using TollTracker.Configuration;
using TollTracker.Enums;
using TollTracker.Extensions;
using TollTracker.Interfaces.Services;
using TollTracker.Metadata;
using TollTracker.Models;

namespace TollTracker.Services;

public class TollService : ITollService
{
    private readonly Uri baseUrl = null!;
    private readonly ILogger<TollService> logger;
    private readonly IReadOnlyCollection<TollPriceModel> priceModels;

    protected HttpClient HttpClient { get; }

    public TollService(HttpClient httpClient, IOptions<HolidayApiOptions> holidayApiOptions, IOptions<TollPriceOptions> tollPriceOptions, ILogger<TollService> logger)
    {
        HttpClient = httpClient;
        baseUrl = holidayApiOptions.Value.BaseUrl;
        priceModels = tollPriceOptions.Value.TestData;
        this.logger = logger;
    }

    public async Task<bool> IsDateExemptFromTollAsync(DateTime date, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(date, nameof(date));

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        string query = date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
        string url = $"{baseUrl}/{query}";
        HttpResponseMessage result;

        try
        {
            result = await HttpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "An error occurred making the HTTP request.");
            throw;
        }

        string content = await result.Content.ReadAsStringAsync(cancellationToken) 
            ?? throw new InvalidOperationException("Failed to deserialize the HTTP content to string");
      
        HolidayInfo? holidayInfo;
        try
        {
            holidayInfo = JsonConvert.DeserializeObject<HolidayInfo>(content);
            if (holidayInfo == null)
            {
                logger.LogError("Deserialized HolidayInfo object is null.");
                throw new InvalidOperationException("Deserialized HolidayInfo object is null.");
            }
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            logger.LogError($"Failed to deserialize the JSON object to a {typeof(HolidayDayInfo).Name}");
            throw;
        }

        return holidayInfo.IsRedDay();
    }

    public bool IsVehicleTypeExemptFromToll(VehicleType type)
    {
        return type.IsExemptFromToll();
    }

    public int CalculateVehicleTollFee(string registrationNumber, IEnumerable<DateTime> datetimes)
    {
        var sortedDates = datetimes.OrderBy(s => s); // Sorting in order to iterate in ascending order. 

        int totalToll = 0;
        int tempToll = 0;
        DateTime? currentInterval = null; // This variable is used to store the start date of the hourly time interval. 

        foreach (var date in sortedDates)
        {
            if(!currentInterval.HasValue || (date - currentInterval.Value).TotalHours > 1)
            {
                if (currentInterval.HasValue) //  Indicating the end of the interval, add the previous toll to the total fee.
                {
                    totalToll += tempToll;
                }
                currentInterval = date; 
                tempToll = GetTollFee(date);
            }
            else
            {
                // Within the same hour. The temporary toll fee should be the maximum toll fee between the current date and starting date of the current interval,
                // This is necessary for the vehicle to be charged once an hour.
                tempToll = Math.Max(tempToll, Math.Max(GetTollFee(date), GetTollFee(currentInterval.Value)));
            }
        }

        // Add the last temporary toll fee to the total fee as foreach doesn't do it.
        totalToll += tempToll;

        return Math.Min(totalToll, 60); // The total fee shall not exceed 60 SEK. 
    }

    private int GetTollFee(DateTime date)
    {
        TimeOnly timeOfDay = TimeOnly.FromDateTime(date);

        return priceModels.FirstOrDefault(t 
            => t.StartTime <= timeOfDay && timeOfDay <= t.EndTime)?.Price ?? 0;
    }
}
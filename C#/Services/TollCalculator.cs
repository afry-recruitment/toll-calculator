using System.Text.Json;
using TollFeeCalculator.Models;
using TollFeeCalculator.Services.Interfaces;

namespace TollFeeCalculator.Services;

public class TollCalculator : ITollCalculator
{
    private readonly SortedDictionary<TimeSpan, TollFeeRule> _tollFeeRules;

    // Constructor to load toll fee rules from a configuration file
    public TollCalculator()
    {
        _tollFeeRules = LoadTollFeeRules("Configuration/tollFeesConfig.json");
    }

    // Method to load and parse toll fee rules from a JSON config file
    private SortedDictionary<TimeSpan, TollFeeRule> LoadTollFeeRules(string configFilePath)
    {
        string json = File.ReadAllText(configFilePath);
        var rules = JsonSerializer.Deserialize<List<TollFeeRule>>(json) ?? new List<TollFeeRule>();

        return MapRulesToDictionary(rules);
    }

    // Helper method to map a list of rules to a SortedDictionary by StartTime
    private SortedDictionary<TimeSpan, TollFeeRule> MapRulesToDictionary(List<TollFeeRule> rules)
    {
        var sortedRules = new SortedDictionary<TimeSpan, TollFeeRule>();

        foreach (var rule in rules)
        {
            sortedRules[rule.StartTime] = rule; // Add or update rule by StartTime
        }

        return sortedRules;
    }

    // Updates the daily toll fee for the provided vehicle based on its passages today
    public void UpdateDailyToll(Vehicle vehicle)
    {
        // Get all today's passages for the vehicle and sort them by time
        var passagesToday = GetTodayPassages(vehicle);

        if (!passagesToday.Any()) return; // Exit if there are no passages today

        int dailyToll = CalculateDailyToll(passagesToday);

        // Cap the daily toll at 60 SEK
        dailyToll = CapDailyToll(dailyToll);

        // Set the calculated toll for the vehicle
        vehicle.DailyTollSetter((dailyToll, DateTime.Today));
    }

    // Method to get today's passages for a given vehicle
    private List<DateTime> GetTodayPassages(Vehicle vehicle)
    {
        return [.. vehicle.Passages
            .Where(p => p.Date == DateTime.Today)
            .OrderBy(p => p.TimeOfDay)];
    }

    // Method to calculate the total daily toll for all passages
    private int CalculateDailyToll(List<DateTime> passages)
    {
        int dailyToll = 0;
        DateTime? lastTollTime = null;

        foreach (var passage in passages)
        {
            dailyToll = ProcessTollForPassage(passage, lastTollTime, dailyToll, out lastTollTime);
        }

        return dailyToll;
    }

    // Method to process a single passage and update the toll
    private int ProcessTollForPassage(DateTime passage, DateTime? lastTollTime, int currentToll, out DateTime? updatedLastTollTime)
    {
        // If more than 60 minutes have passed since the last toll or no last toll, calculate a new toll
        if (lastTollTime == null || (passage - lastTollTime.Value).TotalMinutes > 60)
        {
            int fee = GetTollFee(passage.TimeOfDay);
            currentToll += fee;
            updatedLastTollTime = passage;
        }
        else
        {
            // Only add the highest fee within the same hour
            currentToll = UpdateTollWithinHour(passage, currentToll);
            updatedLastTollTime = lastTollTime; // lastTollTime remains unchanged
        }

        return currentToll;
    }

    // Method to update the toll within the same hour based on the current passage
    private int UpdateTollWithinHour(DateTime passage, int currentToll)
    {
        int currentFee = GetTollFee(passage.TimeOfDay);
        return Math.Max(currentToll, currentFee); // Return the highest toll within the hour
    }

    // Method to ensure the daily toll does not exceed the maximum limit (60 SEK)
    private int CapDailyToll(int dailyToll)
    {
        const int maxDailyToll = 60;
        return Math.Min(dailyToll, maxDailyToll);
    }

    // Retrieves the toll fee for a given passage time based on configured rules
    public int GetTollFee(TimeSpan currentTime)
    {
        foreach (var rule in _tollFeeRules.Values)
        {
            if (IsTimeInRange(currentTime, rule.StartTime, rule.EndTime))
            {
                return rule.Fee;
            }
        }

        return 0; // Return zero if no applicable rule is found
    }

    // Method to check if the current time falls within the given start and end times
    private bool IsTimeInRange(TimeSpan currentTime, TimeSpan startTime, TimeSpan endTime)
    {
        // If startTime <= endTime, the range is continuous during the day
        if (startTime <= endTime)
        {
            return currentTime >= startTime && currentTime <= endTime;
        }

        // If startTime > endTime, the range wraps around midnight
        return currentTime >= startTime || currentTime <= endTime;
    }
}

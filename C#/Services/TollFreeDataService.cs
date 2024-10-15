using System.Text.Json;
using TollFeeCalculator.Services.Interfaces;

namespace TollFeeCalculator.Services;

public class TollFreeDataService : ITollFreeDataService
{
    public List<string> LoadTollFreeVehicles(string configFilePath)
    {
        string json = File.ReadAllText(configFilePath);
        return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
    }

    public List<DateTime> LoadTollFreeDates(string configFilePath)
    {
        string json = File.ReadAllText(configFilePath);
        return JsonSerializer.Deserialize<List<DateTime>>(json) ?? new List<DateTime>();
    }
}

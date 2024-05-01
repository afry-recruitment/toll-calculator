using Newtonsoft.Json;
using Tollculator.WorkingDayChecker.Models;

namespace Tollculator.WorkingDayChecker;

public class WorkingDayChecker : IWorkingDayChecker
{
    /// <summary>
    /// Check if the day is toll free by calling an api.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the day is toll free.</returns>
    public async Task<bool> IsTollFree(DateOnly date)
    {
        using HttpClient client = new();
        MetaData root = null!;
        var path = $"https://sholiday.faboul.se/dagar/v2.1/{date.Year}/{date.Month}/{date.Day}";

        HttpResponseMessage response = await client.GetAsync(path);
        if (response.IsSuccessStatusCode)
        {
            var str = await response.Content.ReadAsStringAsync();
            root = JsonConvert.DeserializeObject<MetaData>(str)!;
        }

        if (root is null)
        {
            return false;
        }

        return root.Dagar[0].ArbetsfriDag!.Equals("Ja");
    }
}

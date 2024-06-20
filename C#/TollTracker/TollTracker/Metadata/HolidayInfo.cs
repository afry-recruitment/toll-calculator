using Newtonsoft.Json;

namespace TollTracker.Metadata;

// Used to deserialize data from the external API "https://sholiday.faboul.se/dagar/v2.1". 
public class HolidayInfo
{
    [JsonProperty("cachetid")]
    public string Cachetid { get; init; } = null!;

    [JsonProperty("version")]
    public string Version { get; init; } = null!;

    [JsonProperty("uri")]
    public string Uri { get; init; } = null!;

    [JsonProperty("startdatum")]
    public string StartDate { get; init; } = null!;

    [JsonProperty("slutdatum")]
    public string EndDate { get; init; } = null!;

    [JsonProperty("dagar")]
    public List<HolidayDayInfo> Days { get; init; } = null!;

    internal bool IsRedDay()
    {
        if (Days.Count != 0)
        {
            return Days.First().RedDay.Equals("ja", StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
}

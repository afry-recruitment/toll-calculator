using Newtonsoft.Json;

namespace TollTracker.Metadata;

// Used to deserialize data from the external API "https://sholiday.faboul.se/dagar/v2.1". 
public class HolidayDayInfo
{
    [JsonProperty("datum")]
    public string Datum { get; init; } = null!;

    [JsonProperty("veckodag")]
    public string Weekday { get; init; } = null!;

    [JsonProperty("arbetsfri dag")]
    public string NonWorkingDay { get; init; } = null!;

    [JsonProperty("r\u00f6d dag")]
    public string RedDay { get; init; } = null!;

    [JsonProperty("vecka")]
    public string Week { get; init; } = null!;

    [JsonProperty("dag i vecka")]
    public string DayOfWeek { get; init; } = null!;

    [JsonProperty("dag före arbetsfri helgdag")]
    public string DayBeforeNonWorkingHoliday { get; init; } = null!;

    [JsonProperty("namnsdag")]
    public List<string> NameDays { get; init; } = null!;

    [JsonProperty("flaggdag")]
    public string FlagDay { get; init; } = null!;
}


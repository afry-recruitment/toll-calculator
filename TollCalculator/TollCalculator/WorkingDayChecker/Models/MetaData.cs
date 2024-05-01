using Newtonsoft.Json;
namespace Tollculator.WorkingDayChecker.Models;

public partial class MetaData
{
    [JsonProperty("cachetid")]
    public string? Cachetid { get; set; }

    [JsonProperty("version")]
    public string? Version { get; set; }

    [JsonProperty("uri")]
    public string? Uri { get; set; }

    [JsonProperty("startdatum")]
    public string? Startdatum { get; set; }

    [JsonProperty("slutdatum")]
    public string? Slutdatum { get; set; }

    [JsonProperty("dagar")]
    public Dag[] Dagar { get; set; } = [];
}

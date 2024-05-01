using Newtonsoft.Json;
namespace Tollculator.WorkingDayChecker.Models;

public partial class Dag
{
    [JsonProperty("datum")]
    public string? Datum { get; set; }

    [JsonProperty("veckodag")]
    public string? Veckodag { get; set; }

    [JsonProperty("arbetsfri dag")]
    public string? ArbetsfriDag { get; set; }

    [JsonProperty("röd dag")]
    public string? RödDag { get; set; }

    [JsonProperty("vecka")]
    public string? Vecka { get; set; }

    [JsonProperty("dag i vecka")]
    public string? DagIVecka { get; set; }

    [JsonProperty("helgdag")]
    public string? Helgdag { get; set; }

    [JsonProperty("namnsdag")]
    public object[] Namnsdag { get; set; } = [];

    [JsonProperty("flaggdag")]
    public string? Flaggdag { get; set; }
}

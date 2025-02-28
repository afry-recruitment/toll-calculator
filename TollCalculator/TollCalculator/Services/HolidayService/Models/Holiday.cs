using System.Text.Json.Serialization;

namespace TollFeeCalculator.Services.HolidayService.Models
{
    internal class Holiday
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
        [JsonPropertyName("nationWide")]
        public bool IsNationWide { get; set; }
    }
}

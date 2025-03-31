namespace RoadToll.Application.Models.RoadToll
{
    public class RoadTollConfiguration
    {
        public required string Region { get; set; }
        public required string Currency { get; set; }
        public int MaxDailyFee { get; set; }
        public double TollFreeLimitMinutes { get; set; }
        public required IEnumerable<RoadTollSchedule> RoadTollSchedule { get; set; }
    }
}

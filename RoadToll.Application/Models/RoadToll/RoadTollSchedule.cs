namespace RoadToll.Application.Models.RoadToll
{
    public class RoadTollSchedule
    {
        public required TimeOnly From { get; set; }
        public required TimeOnly To { get; set; }
        public int Cost { get; set; }
    }
}

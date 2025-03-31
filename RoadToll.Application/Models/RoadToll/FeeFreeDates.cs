namespace RoadToll.Application.Models.RoadToll
{
    public class FeeFreeDates
    {
        public required IEnumerable<DayOfWeek> DaySpecific { get; set; }
        public required IEnumerable<DateOnly> IndividualDays { get; set; }
        public required IEnumerable<DateRange> DateRange { get; set; }
    }
}

namespace AFRY.TollCalculator.API.Domain.Entities;

public class TollFeePeriod : AuditableEntity
{
    public int Id { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
    
    public int Fee { get; set; }

    //public bool IsWithinPeriod(TimeSpan time)
    //{
    //    return time >= StartTime && time <= EndTime;
    //}
}

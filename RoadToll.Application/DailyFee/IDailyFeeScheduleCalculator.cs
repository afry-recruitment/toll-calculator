namespace RoadToll.Application.DailyFee
{
    public interface IDailyFeeScheduleCalculator
    {
        int GetTollFee(DateTime date);
    }
}

namespace TollFeeCalculator.Enterprise.Interfaces
{
    public interface IFeeSchedule
    {
        int GetFee(TimeOnly time);
    }
}

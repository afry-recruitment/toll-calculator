namespace TollFeeCalculator.Policies;

public interface ITollFeeSchedule
{
    int GetFee(DateTime date);
}

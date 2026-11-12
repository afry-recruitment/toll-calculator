namespace TollCalculator.Services.Policies;

public interface ITollFeeSchedule
{
    int GetFee(DateTime dateTime);
}
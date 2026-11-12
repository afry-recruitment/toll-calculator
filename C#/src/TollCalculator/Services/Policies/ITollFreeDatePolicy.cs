namespace TollCalculator.Services.Policies;

public interface ITollFreeDatePolicy
{
    bool IsTollFree(DateTime date);
}
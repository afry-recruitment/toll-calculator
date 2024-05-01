namespace Tollculator.WorkingDayChecker;

public interface IWorkingDayChecker
{
    public Task<bool> IsTollFree(DateOnly date);
}

using CSharpFunctionalExtensions;
namespace TollCalculator.TollFee;

public class TollFeeSpan
{
    public TimeSpan StartTimeSpan { get; }
    public TimeSpan EndTimeSpan { get; } 
    public decimal Fee { get; }

    private TollFeeSpan(TimeSpan startTimeSpan, TimeSpan endTimeSpan, decimal fee)
    {
        StartTimeSpan = startTimeSpan;
        EndTimeSpan = endTimeSpan;
        Fee = fee;
    }
    public static Result<TollFeeSpan> Create(TimeSpan startTimeSpan, TimeSpan endTimeSpan, decimal fee)
    {
        return startTimeSpan > endTimeSpan ? 
            Result.Failure<TollFeeSpan>("cannot have start time later than end time") : 
            Result.Success(new TollFeeSpan(startTimeSpan, endTimeSpan, fee));
    }
    
    public void Deconstruct(out TimeSpan stime, out TimeSpan etime, out decimal fee)
    {
        stime = StartTimeSpan;
        etime = EndTimeSpan;
        fee = Fee;
    }

}
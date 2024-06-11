using CSharpFunctionalExtensions;
namespace TollCalculator.TollFee;

public class TollFeeTable
{
    public readonly TollFeeSpan[] TollFees;
    private TollFeeTable(TollFeeSpan[] tollFees)
    {

        TollFees = tollFees.OrderBy(tf => tf.StartTimeSpan).ToArray();
    }

    public static Result<TollFeeTable> Create(TollFeeSpan[] tollFees)
    {
        ArgumentNullException.ThrowIfNull(tollFees);
        return !IsValidTimeTable(tollFees)  ? 
            Result.Failure<TollFeeTable>("cannot create table with overlapping intervals") : 
            Result.Success(new TollFeeTable(tollFees));
    }

    private static bool IsValidTimeTable(TollFeeSpan[] tollFees)
    {
        for (var i = 1; i <  tollFees.Length; i++)
        {
            // Check if the current interval overlaps with the previous interval
            if ( tollFees[i].StartTimeSpan <  tollFees[i - 1].EndTimeSpan)
            {
                return false;
            }
        }
        return true;
    }
        
}
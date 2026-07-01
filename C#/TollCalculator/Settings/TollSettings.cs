using TollCalculator.Models;

namespace TollCalculator.Settings;

/// <summary>
/// Configuration settings for the toll calculator.
/// Override defaults to adjust business rules without changing the code.
/// </summary>
public class TollSettings
{
    private int _maxDailyFee = 60;
    private int _hourWindowMinutes = 60;

    /// <summary>
    /// Maximum toll fee charged per day in SEK. Default is 60.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if value is less than or equal to 0.</exception>
    public int MaxDailyFee
    {
        get => _maxDailyFee;
        set
        {
            if (value <= 0) throw new ArgumentException("MaxDailyFee must be greater than 0.", nameof(value));
            _maxDailyFee = value;
        }
    }

    /// <summary>
    /// The time window in minutes where only the highest fee applies. Default is 60.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if value is less than or equal to 0.</exception>
    public int HourWindowMinutes
    {
        get => _hourWindowMinutes;
        set
        {
            if (value <= 0) throw new ArgumentException("HourWindowMinutes must be greater than 0.", nameof(value));
            _hourWindowMinutes = value;
        }
    }

    /// <summary>
    /// Defines the fee schedule for the toll system.
    /// Each entry specifies a time range and its corresponding fee in SEK.
    /// Modify this list to adjust fees or add new time intervals without changing the calculator logic.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
    public IReadOnlyList<FeeInterval> FeeSchedule
    {
        get => _feeSchedule;
        set => _feeSchedule = value ?? throw new ArgumentNullException(nameof(value), "FeeSchedule cannot be null.");
    }

    private IReadOnlyList<FeeInterval> _feeSchedule = new[]
    {
        new FeeInterval(new TimeOnly( 6,  0), new TimeOnly( 6, 29),  8),
        new FeeInterval(new TimeOnly( 6, 30), new TimeOnly( 6, 59), 13),
        new FeeInterval(new TimeOnly( 7,  0), new TimeOnly( 7, 59), 18),
        new FeeInterval(new TimeOnly( 8,  0), new TimeOnly( 8, 29), 13),
        new FeeInterval(new TimeOnly( 8, 30), new TimeOnly(14, 59),  8),
        new FeeInterval(new TimeOnly(15,  0), new TimeOnly(15, 29), 13),
        new FeeInterval(new TimeOnly(15, 30), new TimeOnly(16, 59), 18),
        new FeeInterval(new TimeOnly(17,  0), new TimeOnly(17, 59), 13),
        new FeeInterval(new TimeOnly(18,  0), new TimeOnly(18, 29),  8),
    };
}
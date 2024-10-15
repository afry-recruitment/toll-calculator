using System;

namespace TollFeeCalculator.Models;

public class TollFeeRule
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int Fee { get; set; }
}

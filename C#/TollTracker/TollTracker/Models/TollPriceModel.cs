using System.ComponentModel.DataAnnotations;
using TollTracker.Interfaces.Models;

namespace TollTracker.Models;

public class TollPriceModel : ITollPriceModel
{
    [Required]
    public TimeOnly StartTime { get; init; }

    [Required]
    public TimeOnly EndTime { get; init; }

    [Required]
    public int Price { get; init; }
}

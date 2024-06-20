using System.ComponentModel.DataAnnotations;
using TollTracker.Interfaces.Models;

namespace TollTracker.Models;

public class TollTrackingModel : ITollTrackingModel
{
    [Required]
    public VehicleModel Vehicle { get; init; } = null!;

    [Required]
    public IReadOnlyCollection<DateTime> Timestamps { get; init; } = null!;

    private int Count => Timestamps.Count;

    public override string ToString() => 
        $"The vehicle with registration number '{Vehicle.RegistrationNumber}', " +
        $"a {Vehicle.Model} {Vehicle.Type} owned by '{Vehicle.Owner}', " +
        $"has passed through tolls '{Count}' times. The total amount to pay is " ;
}

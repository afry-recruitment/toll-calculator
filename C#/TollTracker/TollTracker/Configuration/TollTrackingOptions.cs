using System.ComponentModel.DataAnnotations;
using TollTracker.Models;

namespace TollTracker.Configuration;

public class TollTrackingOptions
{
    public const string SectionName = "TollTracking";

    [Required]
    public IReadOnlyCollection<TollTrackingModel> TestData { get; set; } = null!;
}


using System.ComponentModel.DataAnnotations;

namespace TollTracker.Configuration;

public class HolidayApiOptions
{
    public const string SectionName = "HolidayApi";

    [Required]
    public Uri BaseUrl { get; init; } = null!;
}


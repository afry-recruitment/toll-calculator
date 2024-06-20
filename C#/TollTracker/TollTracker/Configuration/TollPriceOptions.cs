using System.ComponentModel.DataAnnotations;
using TollTracker.Models;

namespace TollTracker.Configuration;

public class TollPriceOptions
{
    public const string SectionName = "TollPrice";

    [Required]
    public IReadOnlyCollection<TollPriceModel> TestData { get; set; } = null!;
}


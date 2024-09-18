using AFRY.TollCalculator.API.Domain.Models;

namespace AFRY.TollCalculator.API.Features.CalculateTollfee;

public class CalculateTollFeeDto
{
    public Vehicle? Vehicle { get; set; }
    public DateTime[]? Dates { get; set; }
}

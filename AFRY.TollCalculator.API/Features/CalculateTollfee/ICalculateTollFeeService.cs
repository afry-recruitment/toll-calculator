using AFRY.TollCalculator.API.Domain.Entities;
using AFRY.TollCalculator.API.Domain.Models;

namespace AFRY.TollCalculator.API.Features.CalculateTollfee;

public interface ICalculateTollFeeService
{
    int GetTollFee(Vehicle vehicle, DateTime[] dates);
    int GetTotalTollFeeForMultipleDays(Vehicle vehicle, DateTime[] dates);
}

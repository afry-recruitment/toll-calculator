using AFRY.TollCalculator.API.Domain.Entities;

namespace AFRY.TollCalculator.API.Features.CalculateTollfee;

public interface ICalculateTollFeeRepository
{
    bool IsDateTollFree(DateTime date);

    TollFeePeriod? GetTollFeePeriod(TimeSpan time);
}

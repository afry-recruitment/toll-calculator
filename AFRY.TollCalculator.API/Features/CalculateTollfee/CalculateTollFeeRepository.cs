
using AFRY.TollCalculator.API.Domain.Entities;
using AFRY.TollCalculator.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace AFRY.TollCalculator.API.Features.CalculateTollfee;

public class CalculateTollFeeRepository(TollCalculatorDbContext context) : ICalculateTollFeeRepository
{
    private readonly TollCalculatorDbContext _context = context;

    public bool IsDateTollFree(DateTime date)
    {
        return _context.TollFreeDates.Any(d => d.Date.Date == date.Date);
    }

    public TollFeePeriod? GetTollFeePeriod(TimeSpan time)
    {
        var temp = _context.TollFeePeriods
                   .FirstOrDefault(p => time >= p.StartTime && time <= p.EndTime);

        return temp;
    }

    
}


using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TollFeeCalculator.Test")]
namespace TollFeeCalculator.Services.HolidayService
{
    internal interface IHolidayService
    {
        /// <summary>
        /// Checks if current datetime is a national holiday
        /// </summary>
        Task<bool> IsHoliday(DateTime date);
    }
}


namespace TollFeeCalculator.Services.HolidayService
{
    /// <summary>
    /// Client to handle holiday integrations
    /// </summary>
    internal interface IHolidayClient
    {
        /// <summary>
        /// Fetches current national holidays
        /// </summary>
        Task<IEnumerable<Models.Holiday>?> FetchHolidays(string countryCode, string languageCode, DateTime startdat, DateTime endDate);
    }
}

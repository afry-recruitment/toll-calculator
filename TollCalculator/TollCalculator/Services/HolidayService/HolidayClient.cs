using System.Text.Json;
using TollFeeCalculator.Services.HolidayService.Models;

namespace TollFeeCalculator.Services.HolidayService
{
    internal class HolidayClient : IHolidayClient
    {
        private readonly ILogger<HolidayClient> _logger;
        private readonly HttpClient _client;

        public HolidayClient(ILogger<HolidayClient> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _client = clientFactory.CreateClient(nameof(HolidayClient));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Models.Holiday>?> FetchHolidays(string countryCode, string languageCode, DateTime startdate, DateTime endDate)
        {
            if(string.IsNullOrWhiteSpace(countryCode))
            {
                throw new NullReferenceException(nameof(countryCode));
            }

            if (string.IsNullOrWhiteSpace(languageCode))
            {
                throw new NullReferenceException(nameof(languageCode));
            }

            try
            {
                var response = await _client.GetAsync($"https://openholidaysapi.org/PublicHolidays?countryIsoCode={countryCode}&languageIsoCode={languageCode}&validFrom={startdate.ToString("yyyy-MM-dd")}&validTo={endDate.ToString("yyyy-MM-dd")}");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                
                if(!string.IsNullOrWhiteSpace(responseBody))
                {
                    var result = JsonSerializer.Deserialize<IEnumerable<Holiday>>(responseBody);
                    return result;
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while fecthing national holidays from openholidaysapi.org");
            }

            return null;
        }
    }
}

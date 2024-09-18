
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using AFRY.TollCalculator.API.Features.CalculateTollfee;
using AFRY.TollCalculator.API.Domain.Models;

namespace AFRY.TollCalculator.API.IntegrationTests;

public class CalculateTollFeeControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public CalculateTollFeeControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CalculateTollFee_ReturnsCorrectTollFee()
    {
        var request = new
        {
            vehicle = new
            {
                vehicleType = "Car"
            },
            dates = new[]
            {
                "2024-09-17T06:15:00",  // Matches the 6:00 AM period (8 fee)
                "2024-09-17T07:30:00",  // Matches the 7:00 AM period (18 fee)
                "2024-09-17T08:45:00",  // Matches the 8:30 AM period (8 fee)
                "2024-09-17T10:45:00",  // Matches the 10:00 AM period (8 fee)
                "2024-09-17T18:45:00"   // No toll fee period (fee = 0)
            }
        };

        var jsonContent = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/CalculateTollFee", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Response content: {errorContent}");
        }

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TollFeeResponse>(responseString);

        Assert.Equal(42, result.TotalTollFee);
    }

    public class TollFeeResponse
    {
        public int TotalTollFee { get; set; }
    }
}
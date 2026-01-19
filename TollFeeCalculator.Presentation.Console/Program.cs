using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TollFeeCalculator.Enterprise.Enums;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var baseUrl = configuration["Api:BaseUrl"]?.Trim();
if (string.IsNullOrWhiteSpace(baseUrl))
{
    Console.WriteLine("Missing configuration: Api:BaseUrl");
    return;
}

var exitKey = string.Empty;

do
{
    using var http = new HttpClient
    {
        BaseAddress = new Uri(baseUrl, UriKind.Absolute)
    };

    using var cts = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) =>
    {
        e.Cancel = true;
        cts.Cancel();
    };

    Console.WriteLine($"Base URL: {http.BaseAddress}");
    Console.WriteLine("Endpoint: POST /api/v1/TollFeeCalculator/GetFee");
    Console.WriteLine();

    var vehicleType = VehicleType.Car;

    Console.WriteLine("Vehicle types: Car, Motorbike, Tractor, Emergency, Diplomat, Foreign, Military");
    Console.Write("Vehicle type: ");
    var vehicleTypeInput = Console.ReadLine();

    Enum.TryParse(vehicleTypeInput, out vehicleType);

    Console.WriteLine();
    Console.WriteLine("Enter dateTimes (one per line). Example: 2026-01-02T06:15:00");
    Console.WriteLine("Empty line to send.");
    var dateTimes = new List<DateTime>();

    while (true)
    {
        Console.Write("> ");
        var line = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(line))
        {
            break;
        }

        if (!DateTime.TryParse(line, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dt) && !DateTime.TryParse(line, out dt))
        {
            Console.WriteLine("Invalid DateTime.");
            continue;
        }

        dateTimes.Add(dt);
    }

    if (!dateTimes.Any())
    {
        Console.WriteLine("No dateTimes entered.");
        return;
    }

    var request = new
    {
        dateTimes,
        vehicle = new
        {
            type = vehicleType
        }
    };

    var url = "api/v1/TollFeeCalculator/GetFee";

    Console.WriteLine();
    Console.WriteLine($"POST {http.BaseAddress}{url}");
    Console.WriteLine("Request JSON:");
    Console.WriteLine(JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true }));
    Console.WriteLine();

    try
    {
        using var response = await http.PostAsJsonAsync(url, request, cancellationToken: cts.Token).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cts.Token).ConfigureAwait(false);

        Console.WriteLine("----------------------------------------------------------------------------");
        Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");
        Console.WriteLine("Body:");
        Console.WriteLine(body);
        Console.WriteLine();
        Console.WriteLine("Press X to exit, any other key to re-run.");
        Console.WriteLine("----------------------------------------------------------------------------");

        exitKey = Console.ReadLine()?.ToLower();
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Request canceled.");
        exitKey = "x";
    }
} while (exitKey != "x");

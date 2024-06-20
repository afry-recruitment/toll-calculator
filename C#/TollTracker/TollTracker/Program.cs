using Microsoft.Extensions.Options;
using System.Text;
using TollTracker.Configuration;
using TollTracker.Extensions;
using TollTracker.Interfaces.Services;
using TollTracker.Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddTollConfigurationOptions(configuration); // Add services and configurations linked to app settings
WebApplication app = builder.Build();

// There is data to retireve from 14th to 19th June 2024, please feel free to modidy the data in "appsettings.Development.json" to see more
DateTime tollDate = new(2024, 6, 19); 

Console.WriteLine($"\nToll calculation for the date {tollDate:yyyy-MM-dd}: {Environment.NewLine}");

ITollService service = app.Services.GetRequiredService<ITollService>();
CancellationToken cancellationToken = new();
bool isDateTollExempt = await service.IsDateExemptFromTollAsync(tollDate, cancellationToken).ConfigureAwait(false); 
if (isDateTollExempt) 
{
    Console.WriteLine($"{Environment.NewLine} Lucky day! No charges apply today due to weekends and holidays.");
    Environment.Exit(0);
}

TollTrackingOptions tollTrackingOptions = app.Services.GetRequiredService<IOptions<TollTrackingOptions>>().Value; 
var filteredDataByDate = tollTrackingOptions.TestData // Retrieve data from app settings based on the selected date.
    .Select(model => new TollTrackingModel()
    {
        Vehicle = model.Vehicle,
        Timestamps = model.Timestamps
        .Where(timestamp => timestamp.Date == tollDate.Date).ToList()
    })
    .Where(model => model.Timestamps.Count != 0)
    .ToList()
    .AsReadOnly();

StringBuilder text = new(); // Using stringBuilder to save all results and print out at the end

if (filteredDataByDate.Count == 0)
{
    text.AppendLine("{Environment.NewLine}No records found for the specified date.");
}

foreach (TollTrackingModel model in filteredDataByDate)
{
    if (service.IsVehicleTypeExemptFromToll(model.Vehicle.Type)) 
    {
        text.AppendLine($"{Environment.NewLine}{model}0 SEK because the vehicle type is exempt from fees.");
    }
    else
    {
        var result = service.CalculateVehicleTollFee(model.Vehicle.RegistrationNumber, model.Timestamps.ToArray());
        text.AppendLine($"{Environment.NewLine}{model}{result} SEK. ");
    }
}

Console.WriteLine(text);
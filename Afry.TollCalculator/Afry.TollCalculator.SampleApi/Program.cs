using Afry.TollCalculator.Services;
using Afry.TollCalculator.Models;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Afry.TollCalculator.Extensions;

var builder = WebApplication.CreateBuilder(args);

var cultureName = builder.Configuration["SampleApi:Culture"];
if (string.IsNullOrWhiteSpace(cultureName))
{
    cultureName = "sv-SE";
}

var culture = CultureInfo.GetCultureInfo(cultureName);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddTollCalculator(builder.Configuration);

var app = builder.Build();

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = [culture],
    SupportedUICultures = [culture]
};

app.UseRequestLocalization(localizationOptions);

app.MapGet("/health", () => Results.Ok("ok"));

app.MapGet("/toll/single", (
    DateTimeOffset at,
    VehicleType vehicleType,
    ITollCalculatorService calculator) =>
{
    var vehicle = new Vehicle(vehicleType);
    var result = calculator.GetTollFee(at, vehicle);

    return result.IsSuccess
        ? Results.Ok(new { amountMinorUnit = result.Value.MinorUnit, currency = result.Value.Currency })
        : Results.BadRequest(new { error = result.Error });
});

app.MapPost("/toll/day", (
    VehicleType vehicleType,
    DateTimeOffset[] passes,
    ITollCalculatorService calculator) =>
{
    var vehicle = new Vehicle(vehicleType);
    var result = calculator.GetTollFee(vehicle, passes);

    return result.IsSuccess
        ? Results.Ok(new { amountMinorUnit = result.Value.MinorUnit, currency = result.Value.Currency })
        : Results.BadRequest(new { error = result.Error });
});

app.Run();

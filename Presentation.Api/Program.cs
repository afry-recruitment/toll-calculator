using Scalar.AspNetCore;
using TollFeeCalculator.Factories;
using TollFeeCalculator.Interfaces;
using TollFeeCalculator.Models;
using TollFeeCalculator.Services;

var builder = WebApplication.CreateBuilder(args);

var tollConfig = builder.Configuration
    .GetSection("TollConfiguration")
    .Get<TollFeeCalculatorConfig>() ?? TollFeeCalculatorConfigFactory.CreateDefault();

var vehicleConfig = builder.Configuration
    .GetSection("VehicleConfiguration")
    .Get<VehicleConfiguration>() ?? new VehicleConfiguration();

builder.Services.AddSingleton(tollConfig);
builder.Services.AddSingleton<IHolidayProvider, HolidayProvider2026>();
builder.Services.AddScoped<ITollFeeCalculator, TollFeeCalculatorService>();

builder.Services.AddSingleton(vehicleConfig);
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddOpenApi();
builder.Services.AddCors();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api/docs");
}

app.UseHttpsRedirection();

// TODO: Cors for testing purposes only, Restrict this in production
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapPost("/api/toll/calculate", (
    CalculateFeeRequest request,
    ITollFeeCalculator calculator,
    IVehicleService vehicleService,
    TollFeeCalculatorConfig config) =>
    {
        if (!request.Passages.Any())
            return Results.BadRequest("Passages are required");
        
        if (request.VehicleType != null && vehicleService.IsTollFree(request.VehicleType))
            return Results.Ok(new CalculateFeeResponse(TotalFee: 0, Currency: config.Currency));

        var fee = calculator.CalculateFee(request.Passages);

        return Results.Ok(new CalculateFeeResponse(TotalFee: fee, Currency: config.Currency));
    })
    .WithName("CalculateTollFee")
    .WithSummary("Calculate toll fee for vehicle passages")
    .WithDescription("Calculates the total toll fee based on passage timestamps. " +
                     "Applies daily cap (60 SEK), 60-minute rolling windows, and checks for toll-free vehicle types.");


app.Run();

/// <summary>
/// Request to calculate toll fees
/// </summary>
/// <param name="Passages">List of passage timestamps (can span multiple days)</param>
/// <param name="VehicleType">Optional vehicle type (e.g., "Motorcycle", "Bus"). If toll-free, returns 0.</param>
record CalculateFeeRequest(List<DateTime> Passages, string? VehicleType);

/// <summary>
/// Toll fee calculation result
/// </summary>
/// <param name="TotalFee">Total fee amount</param>
/// <param name="Currency">Currency code (e.g., "SEK")</param>
record CalculateFeeResponse(int TotalFee, string Currency);
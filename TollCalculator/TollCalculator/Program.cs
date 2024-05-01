// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tollculator.Calculator;
using Tollculator.Generators;
using Tollculator.Services;
using Tollculator.VehicleRegister;
using Tollculator.WorkingDayChecker;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IWorkingDayChecker, WorkingDayChecker>();
builder.Services.AddSingleton<ITollCalculator, TollCalculator>();
builder.Services.AddSingleton<IVehicleService, VehicleService>();

using IHost host = builder.Build();

var tollCalculator = host.Services.GetRequiredService<ITollCalculator>();

var dictionary = SampleDataParser.ParseLogFile("input.txt");
int i = 0;
foreach (var item in dictionary)
{
    i++;
    var result = await tollCalculator.GetTollFee(new DateOnly(2024, 05, 02), item.Key, item.Value);
    var vehicleType = Register.Vehicles[item.Key];
    Console.WriteLine($"Vehicle of type {vehicleType} with regnr {item.Key} passed the toll {item.Value.Count} times. Price: {result}  " + i);
}
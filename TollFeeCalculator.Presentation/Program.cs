using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Application.Options;
using TollFeeCalculator.Application.Providers;
using TollFeeCalculator.Application.Services;
using TollFeeCalculator.Enterprise.Interfaces;
using TollFeeCalculator.Enterprise.Policies;
using TollFeeCalculator.Enterprise.ValueObjects;
using TollFeeCalculator.Infrastructure.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.Configure<TollFeeOptions>(builder.Configuration.GetSection(nameof(TollFeeOptions)));
builder.Services.AddSingleton<IFeeRangeProvider, GothenburgFeeRangeProvider>();
builder.Services.AddSingleton<IFeeSchedule, FeeSchedule>();
builder.Services.AddSingleton<IVehicleService, VehicleService>();
builder.Services.AddSingleton<IHolidayProvider, SwedenHolidayProvider>();
builder.Services.AddScoped<ITollFeeService, TollFeeService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using TollFeeCalculator.Models.Configuration;
using TollFeeCalculator.Services;
using TollFeeCalculator.Services.HolidayService;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

configuration
    .AddEnvironmentVariables()
    .Build();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();

var memoryCache = new MemoryCache(new MemoryCacheOptions());
builder.Services.AddSingleton<IMemoryCache>(memoryCache);

builder.Services.Configure<TollSettings>(configuration.GetSection("Toll"));
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
builder.Services.AddSingleton<IHolidayClient, HolidayClient>();
builder.Services.AddSingleton<IHolidayService, HolidayService>();
builder.Services.AddSingleton<ITollService, TollService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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

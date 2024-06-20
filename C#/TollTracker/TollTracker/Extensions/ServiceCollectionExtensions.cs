using System.Diagnostics.CodeAnalysis;
using TollTracker.Configuration;
using TollTracker.Interfaces.Services;
using TollTracker.Services;

namespace TollTracker.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddTollConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        services.AddHttpClient();
        services.AddSingleton<ITollService, TollService>();

        services.AddOptions<TollTrackingOptions>()
            .Bind(configuration.GetSection(TollTrackingOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<HolidayApiOptions>()
            .Bind(configuration.GetSection(HolidayApiOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<TollPriceOptions>()
           .Bind(configuration.GetSection(TollPriceOptions.SectionName))
           .ValidateDataAnnotations()
           .ValidateOnStart();
    }
}


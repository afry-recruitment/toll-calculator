using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Afry.TollCalculator.Services;
using Afry.TollCalculator.Infrastructure.Settings;

namespace Afry.TollCalculator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTollCalculator(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<TollSettings>, TollSettingsValidator>();

        services.AddOptions<TollSettings>()
            .Configure(o =>
            {
                configuration.GetSection("TollSettings").Bind(o);
            })
            .ValidateOnStart();

        services.AddSingleton<ITollFreeDateService, TollFreeDateService>();
        services.AddSingleton<ITollFeeScheduleProvider, JsonTollFeeScheduleProvider>();
        services.AddSingleton<ITollCalculatorService, TollCalculatorService>();

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using TollCalculator.Services;
using TollCalculator.Settings;

namespace TollCalculator;

/// <summary>
/// Extension methods for registering TollCalculator services with dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers TollCalculator and its dependencies with the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="settings">Optional custom settings. Uses defaults if not provided.</param>
    public static IServiceCollection AddTollCalculator(
        this IServiceCollection services,
        TollSettings? settings = null)
    {
        services.AddSingleton<HolidayService>();
        services.AddSingleton(settings ?? new TollSettings());
        services.AddSingleton<TollCalculator>();

        return services;
    }
}
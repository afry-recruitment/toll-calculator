using AFRY.TollCalculator.API.Features.CalculateTollfee;
using AFRY.TollCalculator.API.Persistence;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace AFRY.TollCalculator.API;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICalculateTollFeeRepository, CalculateTollFeeRepository>();
        services.AddScoped<ICalculateTollFeeService, CalculateTollFeeService>();

        return services; 
    }

    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TollCalculatorDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TollCalculatorDbConnection")));

        return services;
    }
}

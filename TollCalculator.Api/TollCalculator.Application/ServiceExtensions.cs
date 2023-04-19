using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ServiceExtensions;

public static class ServiceExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}
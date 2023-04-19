using Application.Interfaces.GetTollFee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TollCalculator.Domain.Entities;
using TollCalculator.Infrastructure.Context;
using TollCalculator.Infrastructure.Repositories;

namespace Infrastructure.ServiceRegistration;

public static class ServiceRegistration
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<InMemoryDbContext>(options => options.UseInMemoryDatabase(databaseName: "TollFeesDb"));

        #region Repositories
        services.AddTransient<IGetTollFeeRepository, GetTollFeeRepository>();
        #endregion Repositories
    }
}

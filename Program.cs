using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using tollCalculator.Data;
using tollCalculator.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace TollFeeCalculator;
internal class Program
{
	private static void Main(string[] args)
	{
		var host = Host.CreateDefaultBuilder(args)
			.ConfigureServices((context, services) =>
			{
				var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(connectionString));

				// Registrera din service
				//services.AddTransient<GenericMethodsDb>();
			})
			.Build();
	}
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebWallet.Application.Common.Interfaces;

namespace WebWallet.Persistence
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<WebWalletDbContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("WebWalletDatabase")));

			services.AddScoped<IWebWalletDbContext>(provider => provider.GetService<WebWalletDbContext>());

			return services;
		}
	}
}

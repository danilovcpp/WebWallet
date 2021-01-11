using Microsoft.Extensions.DependencyInjection;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Common.ExchangeClient;
using WebWallet.Infrastructure.Services;

namespace WebWallet.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services)
		{
			services.AddMemoryCache();

			services.AddTransient<IDateTime, DateTimeService>();
			services.AddHttpClient<ExchangeClient>();
			services.AddScoped<IExchangeRateService, CachedExchangeRateService>();

			return services;
		}
	}
}

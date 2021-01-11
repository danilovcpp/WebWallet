using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Common.ExchangeClient;

namespace WebWallet.Infrastructure.Services
{
	public class CachedExchangeRateService : IExchangeRateService
	{
		private readonly ExchangeClient _client;
		private readonly IMemoryCache _cache;

		public CachedExchangeRateService(ExchangeClient client, IMemoryCache cache)
		{
			_client = client;
			_cache = cache;
		}

		public async Task<decimal> GetRate(string sourceCurrency, string targetCurrency)
		{
			var key = $"{sourceCurrency}/{targetCurrency}";

			if (!_cache.TryGetValue(key, out decimal rate))
			{
				var rates = await _client.GetRate(sourceCurrency, targetCurrency);
				rate = _cache.Set(key, rates.Rates[targetCurrency], new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
			}

			return rate;
		}
	}
}

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebWallet.Common.ExchangeClient.Dtos;

namespace WebWallet.Common.ExchangeClient
{
	public class ExchangeClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<ExchangeClient> _logger;

		public ExchangeClient(HttpClient client, ILogger<ExchangeClient> logger)
		{
			_client = client;
			_logger = logger;
		}

		public async Task<RatesResponseDto> GetRate(string @base, string symbols)
		{
			var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

			queryString.Add("base", @base);
			queryString.Add("symbols", symbols);

			var builder = new UriBuilder("https://api.exchangeratesapi.io/latest");
			builder.Query = queryString.ToString();

			_logger.LogInformation("Send GET Request to {Uri}", builder.Uri);

			var response = await _client.GetAsync(builder.Uri)
				.ConfigureAwait(false);

			var data = await response.Content.ReadAsStringAsync();
			_logger.LogInformation("Response data {Data}", data);

			return JsonConvert.DeserializeObject<RatesResponseDto>(data);
		}
	}
}

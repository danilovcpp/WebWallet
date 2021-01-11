using System.Threading.Tasks;

namespace WebWallet.Application.Common.Interfaces
{
	public interface IExchangeRateService
	{
		Task<decimal> GetRate(string sourceCurrency, string targetCurrency);
	}
}

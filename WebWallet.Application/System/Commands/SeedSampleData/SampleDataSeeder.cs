using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Domain.Entities;

namespace WebWallet.Application.System.Commands.SeedSampleData
{
	public class SampleDataSeeder
	{
		private readonly IWebWalletDbContext _context;

		public SampleDataSeeder(IWebWalletDbContext context)
		{
			_context = context;
		}

		public async Task SeedAllAsync(CancellationToken cancellationToken)
		{
			if (_context.Customers.Any())
				return;

			await SeedCustomersAsync(cancellationToken);
			await SeedCurrenciesAsync(cancellationToken);
		}

		public async Task SeedCustomersAsync(CancellationToken cancellationToken)
		{
			var customers = new[]
			{
				new Customer { FirstName = "John", LastName = "Doe", Wallet = new Wallet() },
				new Customer { FirstName = "Jane", LastName = "Doe", Wallet = new Wallet() }
			};

			_context.Customers.AddRange(customers);

			await _context.SaveChangesAsync(cancellationToken);
		}

		public async Task SeedCurrenciesAsync(CancellationToken cancellationToken)
		{
			var currencies = new[]
			{
				new Currency { Name = "RUB", DisplayName = "Российский рубль" },
				new Currency { Name = "USD", DisplayName = "Доллар США" },
				new Currency { Name = "EUR", DisplayName = "Евро" },
				new Currency { Name = "CAD", DisplayName = "Канадский доллар" },
				new Currency { Name = "IDR", DisplayName = "Индонезийская рупия" },
			};

			_context.Currencies.AddRange(currencies);

			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}

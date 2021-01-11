using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebWallet.Domain.Entities;

namespace WebWallet.Application.Common.Interfaces
{
	public interface IWebWalletDbContext
	{
		DbSet<Customer> Customers { get; set; }
		DbSet<Currency> Currencies { get; set; }
		DbSet<CurrencyEntry> CurrencyEntries { get; set; }
		DbSet<Wallet> Wallets { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}

using Microsoft.EntityFrameworkCore;

namespace WebWallet.Persistence
{
	public class WebWalletDbContextFactory : DesignTimeDbContextFactoryBase<WebWalletDbContext>
	{
		protected override WebWalletDbContext CreateNewInstance(DbContextOptions<WebWalletDbContext> options)
		{
			return new WebWalletDbContext(options);
		}
	}
}

using System;
using WebWallet.Persistence;

namespace WebWallet.Application.Tests.Common
{
	public class CommandTestBase : IDisposable
	{
		protected readonly WebWalletDbContext _context;

		public CommandTestBase()
		{
			_context = WebWalletDbContextFactory.Create();
		}

		public void Dispose()
		{
			WebWalletDbContextFactory.Destroy(_context);
		}
	}
}

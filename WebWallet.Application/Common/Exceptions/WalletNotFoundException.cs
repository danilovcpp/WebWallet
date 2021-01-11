using System;

namespace WebWallet.Application.Common.Exceptions
{
	public class WalletNotFoundException : Exception
	{
		public WalletNotFoundException(Guid walletId)
			: base($"Cannot found wallet with id = {walletId}")
		{
		}
	}
}

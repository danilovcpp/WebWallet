using System;

namespace WebWallet.Application.Common.Exceptions
{
	public class CustomerNotFoundException : Exception
	{
		public CustomerNotFoundException(Guid walletId)
			: base($"Cannot found customer with id = {walletId}")
		{
		}
	}
}

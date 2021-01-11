using System;

namespace WebWallet.Application.Common.Exceptions
{
	public class CurrencyEntryNotFoundException : Exception
	{
		public CurrencyEntryNotFoundException()
			: base("Currency entry not found!")
		{
		}
	}
}

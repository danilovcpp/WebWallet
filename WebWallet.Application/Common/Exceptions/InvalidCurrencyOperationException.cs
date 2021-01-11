using System;

namespace WebWallet.Application.Common.Exceptions
{
	public class InvalidCurrencyOperationException : Exception
	{
		public InvalidCurrencyOperationException()
			: base("Operation cannot be performed")
		{
		}
	}
}

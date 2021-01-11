using System;
using System.Collections.Generic;

namespace WebWallet.Domain.Entities
{
	public class Wallet
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
		public Customer Customer { get; set; }

		public ICollection<CurrencyEntry> CurrencyEntries { get; set; }

		public Wallet()
		{
			CurrencyEntries = new List<CurrencyEntry>();
		}
	}
}

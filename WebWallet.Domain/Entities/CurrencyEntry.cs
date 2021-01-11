using System;
using WebWallet.Domain.Common;

namespace WebWallet.Domain.Entities
{
	public class CurrencyEntry : AuditableEntity
	{
		public Guid Id { get; set; }
		public decimal Amount { get; set; }
		public Guid CurrencyId { get; set; }
		public Currency Currency { get; set; }

		public Guid WalletId { get; set; }
		public Wallet Wallet { get; set; }
	}
}

using System;
using System.Collections.Generic;

namespace WebWallet.Application.Wallets.Queries.GetCustomerWallet
{
	public class CustomerWalletVm
	{
		public Guid WalletId { get; set; }
		public IList<CurrencyEntryDto> CurrencyEntries { get; set; }
	}
}

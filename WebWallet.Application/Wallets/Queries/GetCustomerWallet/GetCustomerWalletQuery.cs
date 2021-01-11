using System;
using MediatR;

namespace WebWallet.Application.Wallets.Queries.GetCustomerWallet
{
	public class GetCustomerWalletQuery : IRequest<CustomerWalletVm>
	{
		public Guid CustomerId { get; set; }

		public GetCustomerWalletQuery(Guid customerId)
		{
			CustomerId = customerId;
		}
	}
}

using System;
using MediatR;

namespace WebWallet.Application.Operations.Commands.WithdrawCommand
{
	public class WithdrawCommand : IRequest
	{
		public Guid WalletId { get; set; }
		public Guid CurrencyId { get; set; }
		public decimal Amount { get; set; }
	}
}

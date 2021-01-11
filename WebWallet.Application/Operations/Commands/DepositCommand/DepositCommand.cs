using System;
using MediatR;

namespace WebWallet.Application.Operations.Commands.DepositCommand
{
	public class DepositCommand : IRequest
	{
		public Guid WalletId { get; set; }
		public Guid CurrencyId { get; set; }
		public decimal Amount { get; set; }
	}
}

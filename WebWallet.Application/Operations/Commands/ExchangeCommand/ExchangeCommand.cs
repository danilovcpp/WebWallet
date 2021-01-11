using System;
using MediatR;

namespace WebWallet.Application.Operations.Commands.ExchangeCommand
{
	public class ExchangeCommand : IRequest
	{
		public Guid WalletId { get; set; }
		public Guid SourceCurrencyId { get; set; }
		public Guid TargetCurrencyId { get; set; }
		public decimal Amount { get; set; }
	}
}

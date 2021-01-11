using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebWallet.Application.Common.Exceptions;
using WebWallet.Application.Common.Interfaces;

namespace WebWallet.Application.Operations.Commands.WithdrawCommand
{
	public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand>
	{
		private readonly IWebWalletDbContext _context;

		public WithdrawCommandHandler(IWebWalletDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(WithdrawCommand request, CancellationToken cancellationToken)
		{
			var wallet = await _context.Wallets
				.Include(w => w.CurrencyEntries)
				.FirstOrDefaultAsync(c => c.Id == request.WalletId, cancellationToken);

			if (wallet == null)
				throw new WalletNotFoundException(request.WalletId);

			var currencyEntry = wallet.CurrencyEntries.FirstOrDefault(e => e.CurrencyId == request.CurrencyId);

			if (currencyEntry == null)
				throw new CurrencyEntryNotFoundException();

			if (currencyEntry.Amount < request.Amount)
				throw new InvalidCurrencyOperationException();

			currencyEntry.Amount -= request.Amount;

			try
			{
				// optimistic update
				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (Exception e)
			{
				// todo: handle unique key constraint and optimistic concurrency exceptions here
				throw;
			}

			return Unit.Value;
		}
	}
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebWallet.Application.Common.Exceptions;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Domain.Entities;

namespace WebWallet.Application.Operations.Commands.DepositCommand
{
	public class DepositCommandHandler : IRequestHandler<DepositCommand>
	{
		private readonly IWebWalletDbContext _context;

		public DepositCommandHandler(IWebWalletDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(DepositCommand request, CancellationToken cancellationToken)
		{
			var wallet = await _context.Wallets
				.Include(w => w.CurrencyEntries)
				.FirstOrDefaultAsync(c => c.Id == request.WalletId, cancellationToken);

			if (wallet == null)
				throw new WalletNotFoundException(request.WalletId);

			var currencyEntry = wallet.CurrencyEntries
				.FirstOrDefault(e => e.CurrencyId == request.CurrencyId);

			if (currencyEntry == null)
			{
				wallet.CurrencyEntries.Add(new CurrencyEntry { CurrencyId = request.CurrencyId, Amount = request.Amount });
			}
			else
			{
				currencyEntry.Amount += request.Amount;
			}

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

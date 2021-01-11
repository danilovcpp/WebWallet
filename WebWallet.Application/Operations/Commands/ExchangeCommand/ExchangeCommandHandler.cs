using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebWallet.Application.Common.Exceptions;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Domain.Entities;

namespace WebWallet.Application.Operations.Commands.ExchangeCommand
{
	public class ExchangeCommandHandler : IRequestHandler<ExchangeCommand>
	{
		private readonly IWebWalletDbContext _context;
		private readonly IExchangeRateService _exchangeRateService;

		public ExchangeCommandHandler(IWebWalletDbContext context, IExchangeRateService exchangeRateService)
		{
			_context = context;
			_exchangeRateService = exchangeRateService;
		}

		public async Task<Unit> Handle(ExchangeCommand request, CancellationToken cancellationToken)
		{
			var wallet = await _context.Wallets
				.Include(w => w.CurrencyEntries)
				.ThenInclude(e => e.Currency)
				.FirstOrDefaultAsync(w => w.Id == request.WalletId, cancellationToken);

			if (wallet == null)
				throw new WalletNotFoundException(request.WalletId);

			var sourceCurrency = wallet.CurrencyEntries.FirstOrDefault(c => c.CurrencyId == request.SourceCurrencyId);
			var targetCurrency = wallet.CurrencyEntries.FirstOrDefault(c => c.CurrencyId == request.TargetCurrencyId);

			if (sourceCurrency == null)
				throw new CurrencyEntryNotFoundException();

			if (sourceCurrency.Amount < request.Amount)
				throw new InvalidCurrencyOperationException();

			if (targetCurrency == null)
			{
				targetCurrency = new CurrencyEntry { CurrencyId = request.TargetCurrencyId, Amount = 0 };
				wallet.CurrencyEntries.Add(targetCurrency);
			}

			var target = _context.Currencies.FirstOrDefault(c => c.Id == request.TargetCurrencyId);
			var targetAmount = await _exchangeRateService.GetRate(sourceCurrency.Currency.Name, target?.Name) * request.Amount;

			sourceCurrency.Amount -= request.Amount;
			targetCurrency.Amount += targetAmount;

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

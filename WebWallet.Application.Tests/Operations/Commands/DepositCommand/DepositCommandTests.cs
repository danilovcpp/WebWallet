using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using WebWallet.Application.Common.Exceptions;
using WebWallet.Application.Operations.Commands.DepositCommand;
using WebWallet.Application.Tests.Common;
using Xunit;

namespace WebWallet.Application.Tests.Operations.Commands.DepositCommand
{
	public class DepositCommandTests : CommandTestBase
	{
		private readonly DepositCommandHandler _sut;

		public DepositCommandTests()
		{
			_sut = new DepositCommandHandler(_context);
		}

		[Fact]
		public async Task Handle_GivenNotExistingId_ThrowsWalletNotFoundException()
		{
			var command = new Application.Operations.Commands.DepositCommand.DepositCommand { WalletId = Guid.NewGuid() };

			await Assert.ThrowsAsync<WalletNotFoundException>(() => _sut.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_GivenValidRequest_ShouldCreateCurrencyEntry()
		{
			var walletId = Guid.Parse("7CDC9E77-CA82-49F7-A400-CC518E717956");
			const int amount = 100;

			var currency = _context.Currencies.FirstOrDefault(c => c.Name == "USD");
			currency.ShouldNotBeNull();

			var command = new Application.Operations.Commands.DepositCommand.DepositCommand
			{
				WalletId = walletId,
				CurrencyId = currency.Id,
				Amount = amount
			};

			await _sut.Handle(command, CancellationToken.None);

			var wallet = _context.Wallets.FirstOrDefault(w => w.Id == walletId);
			var currencyEntry = wallet?.CurrencyEntries.FirstOrDefault(c => c.CurrencyId == currency.Id);

			currencyEntry.ShouldNotBeNull();
			currencyEntry.Amount.ShouldBe(amount);
		}

		[Fact]
		public async Task Handle_GivenValidRequest_ShouldIncreaseAmountExistingCurrencyEntry()
		{
			var walletId = Guid.Parse("3B697FD7-6999-469D-97C9-26D0EC836D4C");
			const int amount = 100;

			var currency = _context.Currencies.FirstOrDefault(c => c.Name == "USD");
			currency.ShouldNotBeNull();

			var wallet = _context.Wallets.FirstOrDefault(w => w.Id == walletId);
			var currencyEntry = wallet?.CurrencyEntries.FirstOrDefault(c => c.CurrencyId == currency.Id);
			currencyEntry.ShouldNotBeNull();

			var previousAmount = currencyEntry.Amount;

			var command = new Application.Operations.Commands.DepositCommand.DepositCommand
			{
				WalletId = walletId,
				CurrencyId = currency.Id,
				Amount = amount
			};

			await _sut.Handle(command, CancellationToken.None);

			currencyEntry.Amount.ShouldBe(previousAmount + amount);
		}
	}
}

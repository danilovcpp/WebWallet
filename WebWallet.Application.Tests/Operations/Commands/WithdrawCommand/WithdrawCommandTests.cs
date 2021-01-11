using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using WebWallet.Application.Common.Exceptions;
using WebWallet.Application.Operations.Commands.WithdrawCommand;
using WebWallet.Application.Tests.Common;
using Xunit;

namespace WebWallet.Application.Tests.Operations.Commands.WithdrawCommand
{
	public class WithdrawCommandTests : CommandTestBase
	{
		private readonly WithdrawCommandHandler _sut;

		public WithdrawCommandTests()
		{
			_sut = new WithdrawCommandHandler(_context);
		}

		[Fact]
		public async Task Handle_GivenNotExistingId_ThrowsWalletNotFoundException()
		{
			var command = new Application.Operations.Commands.WithdrawCommand.WithdrawCommand { WalletId = Guid.NewGuid() };

			await Assert.ThrowsAsync<WalletNotFoundException>(() => _sut.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_GivenNotExistingEntryRequest_ShouldThrowCurrencyEntryNotFoundException()
		{
			var walletId = Guid.Parse("7CDC9E77-CA82-49F7-A400-CC518E717956");
			const int amount = 200;

			var currency = _context.Currencies.FirstOrDefault(c => c.Name == "USD");
			currency.ShouldNotBeNull();

			var command = new Application.Operations.Commands.WithdrawCommand.WithdrawCommand
			{
				WalletId = walletId,
				CurrencyId = currency.Id,
				Amount = amount
			};

			await Assert.ThrowsAsync<CurrencyEntryNotFoundException>(() => _sut.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_GivenNotEnoughFundsRequest_ShouldThrowInvalidCurrencyOperationException()
		{
			var walletId = Guid.Parse("3B697FD7-6999-469D-97C9-26D0EC836D4C");
			const int amount = 200;

			var currency = _context.Currencies.FirstOrDefault(c => c.Name == "USD");
			currency.ShouldNotBeNull();

			var command = new Application.Operations.Commands.WithdrawCommand.WithdrawCommand
			{
				WalletId = walletId,
				CurrencyId = currency.Id,
				Amount = amount
			};

			await Assert.ThrowsAsync<InvalidCurrencyOperationException>(() => _sut.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_GivenEnoughFundsRequest_ShouldDecreaseAmount()
		{
			var walletId = Guid.Parse("3B697FD7-6999-469D-97C9-26D0EC836D4C");
			const int amount = 50;

			var currency = _context.Currencies.FirstOrDefault(c => c.Name == "USD");
			currency.ShouldNotBeNull();

			var wallet = _context.Wallets.FirstOrDefault(w => w.Id == walletId);
			var currencyEntry = wallet?.CurrencyEntries.FirstOrDefault(c => c.CurrencyId == currency.Id);
			currencyEntry.ShouldNotBeNull();

			var previousAmount = currencyEntry.Amount;

			var command = new Application.Operations.Commands.WithdrawCommand.WithdrawCommand
			{
				WalletId = walletId,
				CurrencyId = currency.Id,
				Amount = amount
			};

			await _sut.Handle(command, CancellationToken.None);

			currencyEntry.Amount.ShouldBe(previousAmount - amount);
		}
	}
}

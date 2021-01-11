using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using WebWallet.Application.Common.Exceptions;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Application.Operations.Commands.ExchangeCommand;
using WebWallet.Application.Tests.Common;
using Xunit;

namespace WebWallet.Application.Tests.Operations.Commands.ExchangeCommand
{
	public class ExchangeCommandTests : CommandTestBase
	{
		private readonly ExchangeCommandHandler _sut;
		private const decimal DummyRate = 75;

		public ExchangeCommandTests()
		{
			var exchangeMock = new Mock<IExchangeRateService>();
			exchangeMock.Setup(m => m.GetRate(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(DummyRate);

			_sut = new ExchangeCommandHandler(_context, exchangeMock.Object);
		}

		[Fact]
		public async Task Handle_GivenNotExistingId_ThrowsWalletNotFoundException()
		{
			var command = new Application.Operations.Commands.ExchangeCommand.ExchangeCommand { WalletId = Guid.NewGuid() };

			await Assert.ThrowsAsync<WalletNotFoundException>(() => _sut.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_GivenNotExistingEntryRequest_ShouldThrowCurrencyEntryNotFoundException()
		{
			var walletId = Guid.Parse("7CDC9E77-CA82-49F7-A400-CC518E717956");
			const int amount = 200;

			var usdCurrency = _context.Currencies.FirstOrDefault(c => c.Name == "USD");
			usdCurrency.ShouldNotBeNull();

			var rubCurrency = _context.Currencies.FirstOrDefault(c => c.Name == "RUB");
			rubCurrency.ShouldNotBeNull();

			var command = new Application.Operations.Commands.ExchangeCommand.ExchangeCommand
			{
				WalletId = walletId,
				SourceCurrencyId = usdCurrency.Id,
				TargetCurrencyId = rubCurrency.Id,
				Amount = amount
			};

			await Assert.ThrowsAsync<CurrencyEntryNotFoundException>(() => _sut.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_GivenNotEnoughFundsRequest_ShouldThrowInvalidCurrencyOperationException()
		{
			var walletId = Guid.Parse("3B697FD7-6999-469D-97C9-26D0EC836D4C");
			const int amount = 200;

			var usdCurrency = _context.Currencies.FirstOrDefault(c => c.Name == "USD");
			usdCurrency.ShouldNotBeNull();

			var rubCurrency = _context.Currencies.FirstOrDefault(c => c.Name == "RUB");
			rubCurrency.ShouldNotBeNull();

			var command = new Application.Operations.Commands.ExchangeCommand.ExchangeCommand
			{
				WalletId = walletId,
				SourceCurrencyId = usdCurrency.Id,
				TargetCurrencyId = rubCurrency.Id,
				Amount = amount
			};

			await Assert.ThrowsAsync<InvalidCurrencyOperationException>(() => _sut.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_GivenNotExistTargetCurrencyRequest_ShouldCreateCurrencyEntry()
		{
			var walletId = Guid.Parse("3B697FD7-6999-469D-97C9-26D0EC836D4C");
			const int amount = 10;

			var wallet = _context.Wallets
				.Include(w => w.CurrencyEntries)
				.FirstOrDefault(w => w.Id == walletId);

			wallet.ShouldNotBeNull();

			var usdCurrency = _context.Currencies.FirstOrDefault(c => c.Name == "USD");
			usdCurrency.ShouldNotBeNull();

			var rubCurrency = _context.Currencies.FirstOrDefault(c => c.Name == "RUB");
			rubCurrency.ShouldNotBeNull();

			var command = new Application.Operations.Commands.ExchangeCommand.ExchangeCommand
			{
				WalletId = walletId,
				SourceCurrencyId = usdCurrency.Id,
				TargetCurrencyId = rubCurrency.Id,
				Amount = amount
			};

			await _sut.Handle(command, CancellationToken.None);

			wallet.CurrencyEntries.Count.ShouldBe(2);
		}

		[Fact]
		public async Task Handle_GivenValidRequest_ShouldExchangeCorrectly()
		{
			var walletId = Guid.Parse("3B697FD7-6999-469D-97C9-26D0EC836D4C");
			const int amount = 10;

			var wallet = _context.Wallets
				.Include(w => w.CurrencyEntries)
				.FirstOrDefault(w => w.Id == walletId);

			wallet.ShouldNotBeNull();

			var usdCurrency = _context.Currencies.FirstOrDefault(c => c.Name == "USD");
			usdCurrency.ShouldNotBeNull();

			var rubCurrency = _context.Currencies.FirstOrDefault(c => c.Name == "RUB");
			rubCurrency.ShouldNotBeNull();

			var command = new Application.Operations.Commands.ExchangeCommand.ExchangeCommand
			{
				WalletId = walletId,
				SourceCurrencyId = usdCurrency.Id,
				TargetCurrencyId = rubCurrency.Id,
				Amount = amount
			};

			await _sut.Handle(command, CancellationToken.None);

			var rubCurrencyEntry = wallet.CurrencyEntries.FirstOrDefault(c => c.CurrencyId == rubCurrency.Id);

			rubCurrencyEntry.ShouldNotBeNull();
			rubCurrencyEntry.Amount.ShouldBe(DummyRate * amount);
		}
	}
}

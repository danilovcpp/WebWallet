using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Domain.Entities;
using Xunit;

namespace WebWallet.Persistence.Tests
{
	public class WebWalletDbContextTests : IDisposable
	{
		private readonly DateTime _dateTime;
		private readonly Mock<IDateTime> _dateTimeMock;
		private readonly WebWalletDbContext _sut;

		public WebWalletDbContextTests()
		{
			_dateTime = new DateTime(2000, 1, 1);
			_dateTimeMock = new Mock<IDateTime>();
			_dateTimeMock.Setup(m => m.UtcNow).Returns(_dateTime);

			var options = new DbContextOptionsBuilder<WebWalletDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			_sut = new WebWalletDbContext(options, _dateTimeMock.Object);

			var currencies = new[]
			{
				new Currency { Name = "USD",DisplayName = "Доллар США" },
				new Currency { Name="RUB", DisplayName="Российский рубль" }
			};

			var wallet = new Wallet();
			wallet.CurrencyEntries.Add(new CurrencyEntry());

			_sut.Currencies.AddRange(currencies);
			_sut.Wallets.Add(wallet);

			_sut.SaveChanges();
		}

		[Fact]
		public async Task SaveChangesAsync_GivenNewCurrencyEntry_ShouldSetCreatedAtProperty()
		{
			var currencyEntry = new CurrencyEntry();

			_sut.CurrencyEntries.Add(currencyEntry);

			await _sut.SaveChangesAsync();

			currencyEntry.CreatedAt.ShouldBe(_dateTime);
		}


		[Fact]
		public async Task SaveChangesAsync_GivenExistingCurrencyEntry_ShouldSetUpdatedAtProperty()
		{
			var wallet = _sut.Wallets.First();

			var currencyEntry = wallet.CurrencyEntries.First();
			currencyEntry.Amount += 10;

			await _sut.SaveChangesAsync();

			currencyEntry.UpdatedAt.ShouldNotBeNull();
			currencyEntry.UpdatedAt.ShouldBe(_dateTime);
		}

		[Fact]
		public async Task SaveChangesAsync_GivenExistingCurrencyEntry_ShouldUpdateConcurrencyToken()
		{
			var wallet = _sut.Wallets.First();

			var currencyEntry = wallet.CurrencyEntries.First();
			var previousConcurrencyToken = currencyEntry.ConcurrencyToken;

			currencyEntry.Amount += 10;

			await _sut.SaveChangesAsync();

			currencyEntry.ConcurrencyToken.ShouldNotBe(previousConcurrencyToken);
		}

		public void Dispose()
		{
			_sut?.Dispose();
		}
	}
}

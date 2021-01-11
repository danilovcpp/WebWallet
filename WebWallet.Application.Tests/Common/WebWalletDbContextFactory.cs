using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Domain.Entities;
using WebWallet.Persistence;

namespace WebWallet.Application.Tests.Common
{
	public class WebWalletDbContextFactory
	{
		public static WebWalletDbContext Create()
		{
			var options = new DbContextOptionsBuilder<WebWalletDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var dateTimeMock = new Mock<IDateTime>();
			dateTimeMock.Setup(m => m.UtcNow).Returns(new DateTime(2000, 1, 1));

			var context = new WebWalletDbContext(options, dateTimeMock.Object);

			context.Database.EnsureCreated();

			var usdCurrencyId = Guid.Parse("D0492484-74C3-46F1-86BA-CB7BCAF2A57B");
			var rubCurrencyId = Guid.Parse("6984B2AE-718C-460B-A0E6-87750809C62D");

			var currencies = new[]
			{
				new Currency { Id = usdCurrencyId, Name = "USD", DisplayName = "Доллар США" },
				new Currency { Id = rubCurrencyId, Name = "RUB", DisplayName = "Российский рубль"}
			};

			var customers = new[]
			{
				new Customer
				{
					Id = Guid.Parse("569A38D1-1741-4AC5-BB84-A96178D9074F"),
					FirstName = "John",
					LastName = "Doe",
					Wallet = new Wallet { Id = Guid.Parse("7CDC9E77-CA82-49F7-A400-CC518E717956")}
				},
				new Customer
				{
					Id = Guid.Parse("1994E975-6D86-43FF-9CD9-6B8635C075F7"),
					FirstName = "Jane",
					LastName = "Doe",
					Wallet = new Wallet
					{
						Id = Guid.Parse("3B697FD7-6999-469D-97C9-26D0EC836D4C"),
						CurrencyEntries = new List<CurrencyEntry>()
						{
							new CurrencyEntry { CurrencyId = usdCurrencyId, Amount = 100}
						}
					}
				}
			};

			context.Currencies.AddRange(currencies);
			context.Customers.AddRange(customers);

			context.SaveChanges();

			return context;
		}

		public static void Destroy(WebWalletDbContext context)
		{
			context.Database.EnsureDeleted();

			context.Dispose();
		}
	}
}

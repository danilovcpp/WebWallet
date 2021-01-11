using System;
using AutoMapper;
using Shouldly;
using WebWallet.Application.Wallets.Queries.GetCustomerWallet;
using WebWallet.Domain.Entities;
using Xunit;

namespace WebWallet.Application.Tests.Mappings
{
	public class MappingTests : IClassFixture<MappingTestsFixture>
	{
		private readonly IConfigurationProvider _configuration;
		private readonly IMapper _mapper;

		public MappingTests(MappingTestsFixture fixture)
		{
			_configuration = fixture.ConfigurationProvider;
			_mapper = fixture.Mapper;
		}

		[Fact]
		public void ShouldHaveValidConfiguration()
		{
			_configuration.AssertConfigurationIsValid();
		}

		[Fact]
		public void ShouldMapCurrencyEntryDto()
		{
			var currencyEntry = new CurrencyEntry { Amount = 10, Currency = new Currency { Id = Guid.NewGuid(), Name = "USD", DisplayName = "Доллар США" } };

			var result = _mapper.Map<CurrencyEntryDto>(currencyEntry);

			result.ShouldNotBeNull();
			result.ShouldBeOfType<CurrencyEntryDto>();
			result.Amount.ShouldBe(currencyEntry.Amount);
			result.Name.ShouldBe(currencyEntry.Currency.Name);
			result.DisplayName.ShouldBe(currencyEntry.Currency.DisplayName);
			result.CurrencyId.ShouldBe(currencyEntry.Currency.Id);
		}
	}
}

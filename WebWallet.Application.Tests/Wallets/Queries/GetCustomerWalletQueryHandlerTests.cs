using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Shouldly;
using WebWallet.Application.Tests.Common;
using WebWallet.Application.Wallets.Queries.GetCustomerWallet;
using WebWallet.Persistence;
using Xunit;

namespace WebWallet.Application.Tests.Wallets.Queries
{
	[Collection("QueryCollection")]
	public class GetCustomerWalletQueryHandlerTests
	{
		private readonly WebWalletDbContext _context;
		private readonly IMapper _mapper;

		public GetCustomerWalletQueryHandlerTests(QueryTestFixture fixture)
		{
			_context = fixture.Context;
			_mapper = fixture.Mapper;
		}

		[Fact]
		public async Task GetCustomerWallet()
		{
			var sut = new GetCustomerWalletQueryHandler(_context, _mapper);

			var result = await sut.Handle(new GetCustomerWalletQuery(Guid.Parse("569A38D1-1741-4AC5-BB84-A96178D9074F")), CancellationToken.None);

			result.ShouldBeOfType<CustomerWalletVm>();
			result.WalletId.ShouldBe(Guid.Parse("7CDC9E77-CA82-49F7-A400-CC518E717956"));
		}
	}
}

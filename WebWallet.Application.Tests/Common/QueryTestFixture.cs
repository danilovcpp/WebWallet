using System;
using AutoMapper;
using WebWallet.Application.Common.Mappings;
using WebWallet.Persistence;
using Xunit;

namespace WebWallet.Application.Tests.Common
{
	public class QueryTestFixture : IDisposable
	{
		public WebWalletDbContext Context { get; private set; }
		public IMapper Mapper { get; private set; }

		public QueryTestFixture()
		{
			Context = WebWalletDbContextFactory.Create();

			var configurationProvider = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});

			Mapper = configurationProvider.CreateMapper();
		}

		public void Dispose()
		{
			WebWalletDbContextFactory.Destroy(Context);
		}
	}

	[CollectionDefinition("QueryCollection")]
	public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}

using AutoMapper;
using WebWallet.Application.Common.Mappings;

namespace WebWallet.Application.Tests.Mappings
{
	public class MappingTestsFixture
	{
		public MappingTestsFixture()
		{
			ConfigurationProvider = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});

			Mapper = ConfigurationProvider.CreateMapper();
		}

		public IConfigurationProvider ConfigurationProvider { get; }

		public IMapper Mapper { get; }
	}
}

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebWallet.Application.Common.Interfaces;

namespace WebWallet.Application.System.Commands.SeedSampleData
{
	public class SeedSampleDataCommand : IRequest
	{
	}

	public class SeedSampleDataCommandHandler : IRequestHandler<SeedSampleDataCommand>
	{
		private readonly IWebWalletDbContext _context;

		public SeedSampleDataCommandHandler(IWebWalletDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(SeedSampleDataCommand request, CancellationToken cancellationToken)
		{
			var seeder = new SampleDataSeeder(_context);
			await seeder.SeedAllAsync(cancellationToken);

			return Unit.Value;
		}
	}
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebWallet.Application.Common.Exceptions;
using WebWallet.Application.Common.Interfaces;

namespace WebWallet.Application.Wallets.Queries.GetCustomerWallet
{
	public class GetCustomerWalletQueryHandler : IRequestHandler<GetCustomerWalletQuery, CustomerWalletVm>
	{
		private readonly IWebWalletDbContext _context;
		private readonly IMapper _mapper;

		public GetCustomerWalletQueryHandler(IWebWalletDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<CustomerWalletVm> Handle(GetCustomerWalletQuery request, CancellationToken cancellationToken)
		{
			var customer = await _context.Customers
				.Include(c => c.Wallet)
				.FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

			if (customer == null)
				throw new CustomerNotFoundException(request.CustomerId);

			var currencyEntries = await _context.CurrencyEntries
				.Where(c => c.WalletId == customer.Wallet.Id)
				.ProjectTo<CurrencyEntryDto>(_mapper.ConfigurationProvider)
				.ToListAsync(cancellationToken);

			return new CustomerWalletVm
			{
				WalletId = customer.Wallet.Id,
				CurrencyEntries = currencyEntries
			};
		}
	}
}

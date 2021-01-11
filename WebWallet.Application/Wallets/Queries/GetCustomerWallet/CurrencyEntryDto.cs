using System;
using AutoMapper;
using WebWallet.Application.Common.Mappings;
using WebWallet.Domain.Entities;

namespace WebWallet.Application.Wallets.Queries.GetCustomerWallet
{
	public class CurrencyEntryDto : IMapFrom<CurrencyEntry>
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public Guid CurrencyId { get; set; }
		public decimal Amount { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<CurrencyEntry, CurrencyEntryDto>()
				.ForMember(c => c.Name, o => o.MapFrom(c => c.Currency.Name))
				.ForMember(c => c.DisplayName, o => o.MapFrom(c => c.Currency.DisplayName))
				.ForMember(c => c.CurrencyId, o => o.MapFrom(c => c.Currency.Id))
				.ForMember(c => c.Amount, o => o.MapFrom(c => c.Amount));
		}
	}
}

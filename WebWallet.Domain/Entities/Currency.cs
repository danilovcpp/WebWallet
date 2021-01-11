using System;

namespace WebWallet.Domain.Entities
{
	public class Currency
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }
	}
}

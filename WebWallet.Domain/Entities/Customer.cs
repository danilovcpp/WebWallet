using System;
using System.Collections.Generic;

namespace WebWallet.Domain.Entities
{
	public class Customer
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Guid UserId { get; set; }

		public Wallet Wallet { get; set; }
	}
}

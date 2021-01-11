using System;

namespace WebWallet.Domain.Common
{
	public class AuditableEntity
	{
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Guid ConcurrencyToken { get; set; }
	}
}

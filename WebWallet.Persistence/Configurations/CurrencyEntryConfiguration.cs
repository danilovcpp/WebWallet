using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebWallet.Domain.Entities;

namespace WebWallet.Persistence.Configurations
{
	public class CurrencyEntryConfiguration : IEntityTypeConfiguration<CurrencyEntry>
	{
		public void Configure(EntityTypeBuilder<CurrencyEntry> builder)
		{
			// wallet can have only one currecy of each type
			builder.HasIndex(c => new { c.CurrencyId, c.WalletId })
				.IsUnique();

			// optimistic concurrency
			builder.Property(c => c.ConcurrencyToken).IsConcurrencyToken();
		}
	}
}

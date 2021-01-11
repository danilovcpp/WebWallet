using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Domain.Common;
using WebWallet.Domain.Entities;

namespace WebWallet.Persistence
{
	public class WebWalletDbContext : DbContext, IWebWalletDbContext
	{
		private readonly IDateTime _dateTime;

		public WebWalletDbContext(DbContextOptions<WebWalletDbContext> options)
			: base(options)
		{
		}

		public WebWalletDbContext(DbContextOptions<WebWalletDbContext> options, IDateTime dateTime)
			: base(options)
		{
			_dateTime = dateTime;
		}

		public DbSet<Customer> Customers { get; set; }
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<CurrencyEntry> CurrencyEntries { get; set; }
		public DbSet<Wallet> Wallets { get; set; }

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.CreatedAt = _dateTime.UtcNow;
						break;
					case EntityState.Modified:
						entry.Entity.UpdatedAt = _dateTime.UtcNow;
						break;
				}

				entry.Entity.ConcurrencyToken = Guid.NewGuid();
			}

			return base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebWalletDbContext).Assembly);
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//	=> optionsBuilder.LogTo(Console.WriteLine);
	}
}

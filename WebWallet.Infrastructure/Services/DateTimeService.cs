using System;
using WebWallet.Application.Common.Interfaces;

namespace WebWallet.Infrastructure.Services
{
	public class DateTimeService : IDateTime
	{
		public DateTime UtcNow => DateTime.UtcNow;
	}
}

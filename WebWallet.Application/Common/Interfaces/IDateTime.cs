using System;

namespace WebWallet.Application.Common.Interfaces
{
	public interface IDateTime
	{
		DateTime UtcNow { get; }
	}
}

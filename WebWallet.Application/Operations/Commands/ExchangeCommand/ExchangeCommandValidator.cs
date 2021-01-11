using FluentValidation;

namespace WebWallet.Application.Operations.Commands.ExchangeCommand
{
	public class ExchangeCommandValidator : AbstractValidator<ExchangeCommand>
	{
		public ExchangeCommandValidator()
		{
			RuleFor(x => x.WalletId).NotNull();
			RuleFor(x => x.SourceCurrencyId).NotNull();
			RuleFor(x => x.TargetCurrencyId).NotNull();
			RuleFor(x => x.Amount).GreaterThan(0);
		}
	}
}

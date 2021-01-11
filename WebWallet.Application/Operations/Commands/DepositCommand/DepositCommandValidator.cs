using FluentValidation;

namespace WebWallet.Application.Operations.Commands.DepositCommand
{
	public class DepositCommandValidator : AbstractValidator<DepositCommand>
	{
		public DepositCommandValidator()
		{
			RuleFor(x => x.WalletId).NotNull();
			RuleFor(x => x.CurrencyId).NotNull();
			RuleFor(x => x.Amount).GreaterThan(0);
		}
	}
}

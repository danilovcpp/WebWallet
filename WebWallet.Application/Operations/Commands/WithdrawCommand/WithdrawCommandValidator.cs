using FluentValidation;

namespace WebWallet.Application.Operations.Commands.WithdrawCommand
{
	public class WithdrawCommandValidator : AbstractValidator<WithdrawCommand>
	{
		public WithdrawCommandValidator()
		{
			RuleFor(x => x.WalletId).NotNull();
			RuleFor(x => x.CurrencyId).NotNull();
			RuleFor(x => x.Amount).GreaterThan(0);
		}
	}
}

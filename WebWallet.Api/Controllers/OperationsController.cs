using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebWallet.Application.Operations.Commands.DepositCommand;
using WebWallet.Application.Operations.Commands.ExchangeCommand;
using WebWallet.Application.Operations.Commands.WithdrawCommand;

namespace WebWallet.Api.Controllers
{
	public class OperationsController : BaseController
	{
		/// <summary>
		/// Метод пополнения кошелька
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Deposit([FromBody] DepositCommand command)
		{
			await Mediator.Send(command);
			return Ok();
		}

		/// <summary>
		/// Метод снятия средств с кошелька
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Withdraw([FromBody] WithdrawCommand command)
		{
			await Mediator.Send(command);
			return Ok();
		}

		/// <summary>
		/// Метод перевода средств из одной валюты в другую
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Exchange([FromBody] ExchangeCommand command)
		{
			await Mediator.Send(command);
			return Ok();
		}
	}
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebWallet.Application.Wallets.Queries.GetCustomerWallet;

namespace WebWallet.Api.Controllers
{
	public class WalletsController : BaseController
	{
		/// <summary>
		/// Метод получения информации о кошельке пользователя
		/// </summary>
		/// <param name="customerId">Идентификатор пользователя</param>
		/// <returns></returns>
		[HttpGet("{customerId:guid}")]
		public async Task<IActionResult> GetWallet(Guid customerId)
		{
			return Ok(await Mediator.Send(new GetCustomerWalletQuery(customerId)));
		}
	}
}

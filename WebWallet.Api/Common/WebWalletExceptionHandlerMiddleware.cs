using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebWallet.Application.Common.Exceptions;

namespace WebWallet.Api.Common
{
	public class WebWalletExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public WebWalletExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			var result = JsonConvert.SerializeObject(new { error = exception.Message });

			return context.Response.WriteAsync(result);
		}
	}

	public static class WebWalletExceptionHandlerMiddlewareExtensions
	{
		public static IApplicationBuilder UseWebWalletExceptionHandler(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<WebWalletExceptionHandlerMiddleware>();
		}
	}
}

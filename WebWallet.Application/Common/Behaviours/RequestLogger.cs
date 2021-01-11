using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace WebWallet.Application.Common.Behaviours
{
	public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
	{
		private readonly ILogger _logger;

		public RequestLogger(ILogger<TRequest> logger)
		{
			_logger = logger;
		}

		public Task Process(TRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("WebWallet Request: {Name} {@Request}", typeof(TRequest).Name, request);

			return Task.CompletedTask;
		}
	}
}

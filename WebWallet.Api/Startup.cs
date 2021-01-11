using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using WebWallet.Api.Common;
using WebWallet.Application;
using WebWallet.Application.Common.Interfaces;
using WebWallet.Infrastructure;
using WebWallet.Persistence;

namespace WebWallet.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddInfrastructure();
			services.AddPersistence(Configuration);
			services.AddApplication();

			services.AddControllers()
				.AddNewtonsoftJson()
				.AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<IWebWalletDbContext>());

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebWallet.Api", Version = "v1" });
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebWallet.Api v1"));
			}

			app.UseWebWalletExceptionHandler();
			app.UseHttpsRedirection();
			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}

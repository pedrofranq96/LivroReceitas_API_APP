using LivroReceitas.Application.UseCases.Login.FazerLogin;
using LivroReceitas.Infra.AcessoRepositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test.V1.ErroDesconhecido;

public class LivroReceitasWebApplicationFactorySemIdLogin<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Test")
			.ConfigureServices(services =>
			{
				var descritor = services.SingleOrDefault(d => d.ServiceType == typeof(Context));
				if (descritor is not null)
					services.Remove(descritor);

				var useCaseLogin = services.SingleOrDefault(d => d.ServiceType == typeof(ILoginUseCase));
				if (useCaseLogin is not null)
					services.Remove(useCaseLogin);

				var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

				services.AddDbContext<Context>(options =>
				{
					options.UseInMemoryDatabase("InMemoryDbForTesting");
					options.UseInternalServiceProvider(provider);
				});

				var serviceProvider = services.BuildServiceProvider();


				using var scope = serviceProvider.CreateScope();
				var scopeService = scope.ServiceProvider;

				var database = scopeService.GetRequiredService<Context>();

				database.Database.EnsureDeleted();
			});
	}
}
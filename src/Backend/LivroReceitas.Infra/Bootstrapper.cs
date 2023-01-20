using FluentMigrator.Runner;
using LivroReceitas.Domain.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;
using System.Reflection;

namespace LivroReceitas.Infra;

public static class Bootstrapper
{
	public static void AddRepositorio(this IServiceCollection services, IConfiguration configurationManager)
	{
		AddFluentMigrator(services, configurationManager);
	}

	private static void AddFluentMigrator(IServiceCollection services, IConfiguration configurationManager)
	{
		services.AddFluentMigratorCore().ConfigureRunner(c => c.AddMySql5()
		.WithGlobalConnectionString(configurationManager.GetConexaoCompleta())
		.ScanIn(Assembly.Load("LivroReceitas.Infra")).For.All());
	}
}

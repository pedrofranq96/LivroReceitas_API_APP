using FluentMigrator.Runner;
using LivroReceitas.Domain.Extension;
using LivroReceitas.Domain.Repositorio;
using LivroReceitas.Infra.AcessoRepositorio;
using LivroReceitas.Infra.AcessoRepositorio.Repositorio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using LivroReceitas.Domain.Repositorio.Usuario;

namespace LivroReceitas.Infra;

public static class Bootstrapper
{


	public static void AddRepositorio(this IServiceCollection services, IConfiguration configurationManager)
	{
		AddFluentMigrator(services, configurationManager);
		AddContext(services, configurationManager);
		AddUnidadeTrabalho(services);
		AddRepositorios(services);
	}
	private static void AddContext(IServiceCollection services, IConfiguration configurationManager)
	{
		bool.TryParse(configurationManager.GetSection("Configuracoes:BancoDeDadosInMemory").Value, out bool bancoDeDadosInMemory);
		if (!bancoDeDadosInMemory)
		{
			var versaoServidor = new MySqlServerVersion(new Version(8, 0, 32));
			var connectionString = configurationManager.GetConexaoCompleta();
			services.AddDbContext<Context>(dbContextOptions =>
			{
				dbContextOptions.UseMySql(connectionString, versaoServidor);
			});
		}		
	}

	private static void AddUnidadeTrabalho(IServiceCollection services)
	{
		services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
	}

	private static void AddRepositorios(IServiceCollection services)
	{
		services.AddScoped<IUsuarioWriteOnlyRepositorio, UsuarioRepositorio>()
			.AddScoped<IUsuarioReadOnlyRepositorio, UsuarioRepositorio>()
			.AddScoped<IUsuarioUpdateOnlyRepositorio, UsuarioRepositorio>();
	}
	private static void AddFluentMigrator(IServiceCollection services, IConfiguration configurationManager)
	{
		bool.TryParse(configurationManager.GetSection("Configuracoes:BancoDeDadosInMemory").Value, out bool bancoDeDadosInMemory);
		if (!bancoDeDadosInMemory)
		{
			services.AddFluentMigratorCore().ConfigureRunner(c => c.AddMySql5()
				.WithGlobalConnectionString(configurationManager.GetConexaoCompleta())
				.ScanIn(Assembly.Load("LivroReceitas.Infra")).For.All());
		}			
	}
}

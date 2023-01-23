using LivroReceitas.Application.Servicos.Criptografia;
using LivroReceitas.Application.Servicos.Token;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Application.UseCases.Login.FazerLogin;
using LivroReceitas.Application.UseCases.Usuario.AlterarSenha;
using LivroReceitas.Application.UseCases.Usuario.Registrar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LivroReceitas.Application;

public static class Bootstrapper
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		AddChaveSenha(services, configuration);
		AddTokenJWT(services, configuration);
		AddUseCases(services);
		AddUsuarioLogado(services);
		
	}

	private static void AddUsuarioLogado(IServiceCollection services)
	{
		services.AddScoped<IUsuarioLogado, UsuarioLogado>();
	}

	private static void AddChaveSenha(this IServiceCollection services, IConfiguration configuration)
	{
		var section = configuration.GetRequiredSection("Configuracoes:ChaveAdicionalSenha");
		services.AddScoped(options => new EncriptadorDeSenha(section.Value));
	}
	private static void AddTokenJWT(this IServiceCollection services, IConfiguration configuration)
	{
		var sectionTempoDeVida = configuration.GetRequiredSection("Configuracoes:TempoDeVidaToken");
		var sectionKey= configuration.GetRequiredSection("Configuracoes:ChaveToken");
		services.AddScoped(options => new TokenController(int.Parse(sectionTempoDeVida.Value), sectionKey.Value));
	}

	private static void AddUseCases(this IServiceCollection services)
	{
		services.AddScoped<IRegistrarUsuarioUseCase, RegistrarUsuarioUseCase>()
			.AddScoped<ILoginUseCase, LoginUseCase>()
			.AddScoped<IAlterarSenhaUseCase, AlterarSenhaUseCase>();
	}
}

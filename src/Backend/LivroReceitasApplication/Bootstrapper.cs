using LivroReceitas.Application.Servicos.Criptografia;
using LivroReceitas.Application.Servicos.Token;
using LivroReceitas.Application.UseCases.Usuario.Registrar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Collections.Specialized.BitVector32;

namespace LivroReceitas.Application;

public static class Bootstrapper
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		AddChaveSenha(services, configuration);
		AddTokenJWT(services, configuration);
		services.AddScoped<IRegistrarUsuarioUseCase, RegistrarUsuarioUseCase>();
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
}

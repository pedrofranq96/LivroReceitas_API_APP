using LivroReceitas.Application.Servicos.Criptografia;
using LivroReceitas.Application.Servicos.Token;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Application.UseCases.Conexao.AceitarConexao;
using LivroReceitas.Application.UseCases.Conexao.GerarQRCode;
using LivroReceitas.Application.UseCases.Conexao.QRCodeLido;
using LivroReceitas.Application.UseCases.Conexao.Recuperar;
using LivroReceitas.Application.UseCases.Conexao.RecusarConexao;
using LivroReceitas.Application.UseCases.Conexao.RemoverConexao;
using LivroReceitas.Application.UseCases.DashBoard;
using LivroReceitas.Application.UseCases.Login.FazerLogin;
using LivroReceitas.Application.UseCases.Receita.Atualizar;
using LivroReceitas.Application.UseCases.Receita.Excluir;
using LivroReceitas.Application.UseCases.Receita.RecuperarPorId;
using LivroReceitas.Application.UseCases.Receita.Registrar;
using LivroReceitas.Application.UseCases.Usuario.AlterarSenha;
using LivroReceitas.Application.UseCases.Usuario.RecuperarPerfil;
using LivroReceitas.Application.UseCases.Usuario.Registrar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LivroReceitas.Application;

public static class Bootstrapper
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		AddChaveSenha(services, configuration);
		AddHashIds(services, configuration);
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
		var section = configuration.GetRequiredSection("Configuracoes:Senha:ChaveAdicionalSenha");
		services.AddScoped(options => new EncriptadorDeSenha(section.Value));
	}
	private static void AddTokenJWT(IServiceCollection services, IConfiguration configuration)
	{
		var sectionTempoDeVida = configuration.GetRequiredSection("Configuracoes:Jwt:TempoDeVidaTokenMinutos");
		var sectionKey= configuration.GetRequiredSection("Configuracoes:Jwt:ChaveToken");
		services.AddScoped(options => new TokenController(int.Parse(sectionTempoDeVida.Value), sectionKey.Value));
	}
	private static void AddHashIds(IServiceCollection services, IConfiguration configuration)
	{
		var salt = configuration.GetRequiredSection("Configuracoes:HashIds:Salt");
		services.AddHashids(setup => 
		{
			setup.Salt = salt.Value;
			setup.MinHashLength = 3;
		});
	}
	private static void AddUseCases(IServiceCollection services)
	{
		services.AddScoped<IRegistrarUsuarioUseCase, RegistrarUsuarioUseCase>()
			.AddScoped<ILoginUseCase, LoginUseCase>()
			.AddScoped<IAlterarSenhaUseCase, AlterarSenhaUseCase>()
			.AddScoped<IRegistrarReceitaUseCase, RegistrarReceitaUseCase>()
			.AddScoped<IDashBoardUseCase, DashBoardUseCase>()
			.AddScoped<IRecuperarReceitaPorIdUseCase, RecuperarReceitaPorIdUseCase>()
			.AddScoped<IReceitaAtualizarUseCase, ReceitaAtualizarUseCase>()
			.AddScoped<IDeletarReceitaUseCase, DeletarReceitaUseCase>()
			.AddScoped<IRecuperarPerfilUseCase, RecuperarPerfilUseCase>()
			.AddScoped<IGerarQRCodeUseCase, GerarQRCodeUseCase>()
			.AddScoped<IQRCodeLidoUseCase, QRCodeLidoUseCase>()
			.AddScoped<IRecusarConexaoUseCase, RecusarConexaoUseCase>()
			.AddScoped<IAceitarConexaoUseCase, AceitarConexaoUseCase>()
			.AddScoped<IRecuperarTodasConexoesUseCase, RecuperarTodasConexoesUseCase>()
			.AddScoped<IRemoverConexaoUseCase, RemoverConexaoUseCase>();


	}
}

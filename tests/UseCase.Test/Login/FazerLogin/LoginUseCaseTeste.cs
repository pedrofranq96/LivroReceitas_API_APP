using FluentAssertions;
using LivroReceitas.Application.UseCases.Login.FazerLogin;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;
using UtilitarioParaOsTestes.Criptografia;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.Repositorios;
using UtilitarioParaOsTestes.Token;
using Xunit;

namespace UseCase.Test.Login.FazerLogin;
public class LoginUseCaseTeste
{

	[Fact]
	public async Task Validar_Sucesso()
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();

		var useCase = CriarUseCase(usuario);

		var resposta = await useCase.Executar(new RequisicaoLoginJson
		{
			Email = usuario.Email,
			Senha = senha
		});
		resposta.Should().NotBeNull();
		resposta.Nome.Should().Be(usuario.Nome);
		resposta.Token.Should().NotBeNullOrWhiteSpace();
	}
	
	[Fact]
	public async Task Validar_Erro_Senha_Invalida()
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();

		var useCase = CriarUseCase(usuario);

		Func<Task> acao = async () =>
		{
			await useCase.Executar(new RequisicaoLoginJson
			{
				Email = usuario.Email,
				Senha = "senhainvalida"
			});
		};

		await acao.Should().ThrowAsync<LoginInvalidoException>()
			.Where(exception => exception.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
		
	}


	[Fact]
	public async Task Validar_Erro_Email_Senha_Invalida()
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();

		var useCase = CriarUseCase(usuario);

		Func<Task> acao = async () =>
		{
			await useCase.Executar(new RequisicaoLoginJson
			{
				Email = "email@invalido.com",
				Senha = "senhainvalida"
			});
		};

		await acao.Should().ThrowAsync<LoginInvalidoException>()
			.Where(exception => exception.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
		
	}
	
	[Fact]
	public async Task Validar_Erro_Email_Invalido()
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();

		var useCase = CriarUseCase(usuario);

		Func<Task> acao = async () =>
		{
			await useCase.Executar(new RequisicaoLoginJson
			{
				Email = "email@Invalido.com",
				Senha = senha
			});
		};

		await acao.Should().ThrowAsync<LoginInvalidoException>()
			.Where(exception => exception.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
		
	}

	private LoginUseCase CriarUseCase(LivroReceitas.Domain.Entidades.Usuario usuario)
	{
		
		var encriptador = EncriptadorDeSenhaBuilder.Instancia();
		var token = TokenControllerBuilder.Instancia();
		var repositoruioReadOnly = UsuarioReadOnlyRepositorioBuilder.Intancia().Login(usuario).Construir();
		return new LoginUseCase(encriptador, token, repositoruioReadOnly);
	}
}

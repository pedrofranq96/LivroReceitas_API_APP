using FluentAssertions;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using UtilitarioParaOsTestes.Requisicoes;
using Xunit;

namespace WebApi.Test.V1.Login.FazerLogin;
public class LoginTest : ControllerBase
{
	private const string METODO = "login";
	private LivroReceitas.Domain.Entidades.Usuario _usuario;
	private string _senha;

	public LoginTest(LivroReceitasWebApplicationFactory<Program> factory) : base(factory) 
	{
		_usuario = factory.RecuperarUsuario();
		_senha = factory.RecuperarSenha();
	}

	[Fact]
	public async Task Validar_Sucesso()
	{
		var requisicao = new RequisicaoLoginJson
		{
			Email = _usuario.Email,
			Senha = _senha
		};
		var resposta = await PostRequest(METODO, requisicao);

		resposta.StatusCode.Should().Be(HttpStatusCode.OK);

		await using var respostaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(respostaBody);

		responseData.RootElement.GetProperty("nome").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_usuario.Nome);
		responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();

	}
	
	[Fact]
	public async Task Validar_Email_Invalido()
	{
		var requisicao = new RequisicaoLoginJson
		{
			Email = "email@invalido.com",
			Senha = _senha
		};
		var resposta = await PostRequest(METODO, requisicao);

		resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

		await using var respostaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(respostaBody);

		var erros = responseData.RootElement.GetProperty("mensagens").Deserialize<List<string>>();
		erros.Should().ContainSingle().And.Contain(ResourceMensagensDeErro.LOGIN_INVALIDO);
	}
	
	[Fact]
	public async Task Validar_Senha_Invalido()
	{
		var requisicao = new RequisicaoLoginJson
		{
			Email = _usuario.Email,
			Senha = "senhaInvalida"
		};
		var resposta = await PostRequest(METODO, requisicao);

		resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

		await using var respostaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(respostaBody);

		var erros = responseData.RootElement.GetProperty("mensagens").Deserialize<List<string>>();
		erros.Should().ContainSingle().And.Contain(ResourceMensagensDeErro.LOGIN_INVALIDO);
	}
	
	
	[Fact]
	public async Task Validar_Email_Senha_Invalido()
	{
		var requisicao = new RequisicaoLoginJson
		{
			Email = "email@invalido.com",
			Senha = "senhaInvalida"
		};
		var resposta = await PostRequest(METODO, requisicao);

		resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

		await using var respostaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(respostaBody);

		var erros = responseData.RootElement.GetProperty("mensagens").Deserialize<List<string>>();
		erros.Should().ContainSingle().And.Contain(ResourceMensagensDeErro.LOGIN_INVALIDO);
	}
}

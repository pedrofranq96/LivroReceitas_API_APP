using FluentAssertions;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using WebApi.Test.V1.ErroDesconhecido;
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

		await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responstaBody);

		responseData.RootElement.GetProperty("nome").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_usuario.Nome);
		responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
	}

	[Theory]
	[InlineData("pt")]
	[InlineData("en")]
	public async Task Validar_Erro_Senha_Invalido(string cultura)
	{
		var requisicao = new RequisicaoLoginJson
		{
			Email = _usuario.Email,
			Senha = "senhaInvalida"
		};

		var resposta = await PostRequest(METODO, requisicao, cultura: cultura);

		resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

		await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responstaBody);

		var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();

		var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("LOGIN_INVALIDO", new System.Globalization.CultureInfo(cultura));
		erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(mensagemEsperada));
	}

	[Theory]
	[InlineData("pt")]
	[InlineData("en")]
	public async Task Validar_Erro_Email_Invalido(string cultura)
	{
		var requisicao = new RequisicaoLoginJson
		{
			Email = "email@invalido.com",
			Senha = _senha
		};

		var resposta = await PostRequest(METODO, requisicao, cultura: cultura);

		resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

		await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responstaBody);

		var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();

		var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("LOGIN_INVALIDO", new System.Globalization.CultureInfo(cultura));
		erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(mensagemEsperada));
	}


	[Theory]
	[InlineData("pt")]
	[InlineData("en")]
	public async Task Validar_Erro_Email_Senha_Invalido(string cultura)
	{
		var requisicao = new RequisicaoLoginJson
		{
			Email = "email@invalido.com",
			Senha = "senhaInvalida"
		};

		var resposta = await PostRequest(METODO, requisicao, cultura: cultura);

		resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

		await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responstaBody);

		var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();

		var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("LOGIN_INVALIDO", new System.Globalization.CultureInfo(cultura));
		erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(mensagemEsperada));
	}
}

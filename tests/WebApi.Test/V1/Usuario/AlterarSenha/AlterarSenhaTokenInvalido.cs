using FluentAssertions;
using System.Net;
using UtilitarioParaOsTestes.Requisicoes;
using UtilitarioParaOsTestes.Token;
using WebApi.Test.V1.ErroDesconhecido;
using Xunit;

namespace WebApi.Test.V1.Usuario.AlterarSenha;
public class AlterarSenhaTokenInvalido : ControllerBase
{
	private const string METODO = "usuario/alterar-senha";
	private LivroReceitas.Domain.Entidades.Usuario _usuario;
	private string _senha;

	public AlterarSenhaTokenInvalido(LivroReceitasWebApplicationFactory<Program> factory) : base(factory)
	{
		_usuario = factory.RecuperarUsuario();
		_senha = factory.RecuperarSenha();
	}

	[Fact]
	public async Task Validar_ErroToken_Vazio()
	{
		var token = string.Empty;

		var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
		requisicao.SenhaAtual = _senha;
		var resposta = await PutRequest(METODO, requisicao, token);

		resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

	}
	
	[Fact]
	public async Task Validar_Token_Usuario_Vazio()
	{
		var token = TokenControllerBuilder.Instancia().GerarToken("usuario@fake.com");

		var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
		requisicao.SenhaAtual = _senha;
		var resposta = await PutRequest(METODO, requisicao, token);

		resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

	}
	
	[Fact]
	public async Task Validar_Token_Expirado()
	{
		var token = TokenControllerBuilder.TokenExpirado().GerarToken(_usuario.Email);
		Thread.Sleep(1000);

		var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
		requisicao.SenhaAtual = _senha;
		var resposta = await PutRequest(METODO, requisicao, token);

		resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

	}

}

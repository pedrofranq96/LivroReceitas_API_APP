using FluentAssertions;
using LivroReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using UtilitarioParaOsTestes.HashIds;
using Xunit;

namespace WebApi.Test.V1.Receita.Deletar;
public class DeletarReceitaTeste : ControllerBase
{
	private const string METODO = "receitas";

	private LivroReceitas.Domain.Entidades.Usuario _usuario;
	private string _senha;

	public DeletarReceitaTeste(LivroReceitasWebApplicationFactory<Program> factory) : base(factory)
	{
		_usuario = factory.RecuperarUsuario();
		_senha = factory.RecuperarSenha();
	}

	[Fact]
	public async Task Validar_Sucesso()
	{
		string cultura = "en";

		var token = await Login(_usuario.Email, _senha);

		var receitaId = await GetReceitaId(token);

		var resposta = await DeleteRequest($"{METODO}/{receitaId}", token, cultura: cultura);

		resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);

		var respostaReceitaId = await GetRequest($"{METODO}/{receitaId}", token, cultura: cultura);

		respostaReceitaId.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		await using var responstaBody = await respostaReceitaId.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responstaBody);

		var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();
		erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
	}

	[Theory]
	[InlineData("pt")]
	[InlineData("en")]
	public async Task Validar_Erro_Receita_Inexistente(string cultura)
	{
		var token = await Login(_usuario.Email, _senha);

		var receitaId = HashidsBuilder.Instance().Build().EncodeLong(0);

		var resposta = await DeleteRequest($"{METODO}/{receitaId}", token, cultura: cultura);

		resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responstaBody);

		var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();
		erros.Should().ContainSingle().And.Contain(x => x.GetString().Equals(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
	}
}


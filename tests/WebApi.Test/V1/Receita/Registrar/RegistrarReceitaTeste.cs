using FluentAssertions;
using LivroReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using UtilitarioParaOsTestes.Requisicoes;
using Xunit;

namespace WebApi.Test.V1.Receita.Registrar;
public class RegistrarReceitaTeste : ControllerBase
{
	private const string METODO = "receitas";

	private LivroReceitas.Domain.Entidades.Usuario _usuario;
	private string _senha;

	public RegistrarReceitaTeste(LivroReceitasWebApplicationFactory<Program> factory) : base(factory)
	{
		_usuario = factory.RecuperarUsuario();
		_senha = factory.RecuperarSenha();
	}

	[Fact]
	public async Task Validar_Sucesso()
	{
		var token = await Login(_usuario.Email, _senha);

		var requisicao = RequisicaoReceitaBuilder.Construir();

		var resposta = await PostRequest(METODO, requisicao, token);

		resposta.StatusCode.Should().Be(HttpStatusCode.Created);

		await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responstaBody);

		responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
		responseData.RootElement.GetProperty("titulo").GetString().Should().Be(requisicao.Titulo);
		responseData.RootElement.GetProperty("categoria").GetUInt16().Should().Be((ushort)requisicao.Categoria);
		responseData.RootElement.GetProperty("modoPreparo").GetString().Should().Be(requisicao.ModoPreparo);
		responseData.RootElement.GetProperty("tempoPreparo").GetInt32().Should().Be(requisicao.TempoPreparo);
	}

	[Theory]
	[InlineData("pt")]
	[InlineData("en")]
	public async Task Validar_Erro_Sem_Ingredientes(string cultura)
	{
		var token = await Login(_usuario.Email, _senha);
		var requisicao = RequisicaoReceitaBuilder.Construir();
		requisicao.Ingredientes.Clear();

		var resposta = await PostRequest(METODO, requisicao, token, cultura: cultura);

		resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responstaBody);

		var erros = responseData.RootElement.GetProperty("mensagens");

		var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("RECEITA_MINIMO_UM_INGREDIENTE", new System.Globalization.CultureInfo(cultura));
		erros.Equals(mensagemEsperada);
		
	}
}

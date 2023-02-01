﻿using FluentAssertions;
using LivroReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using UtilitarioParaOsTestes.HashIds;
using Xunit;

namespace WebApi.Test.V1.Receita.RecuperarPorId;
public class RecuperarReceitaPorIdTeste : ControllerBase
{
	private const string METODO = "receitas";

	private LivroReceitas.Domain.Entidades.Usuario _usuario;
	private string _senha;

	public RecuperarReceitaPorIdTeste(LivroReceitasWebApplicationFactory<Program> factory) : base(factory)
	{
		_usuario = factory.RecuperarUsuario();
		_senha = factory.RecuperarSenha();
	}

	[Fact]
	public async Task Validar_Sucesso()
	{
		var token = await Login(_usuario.Email, _senha);

		var receitaId = await GetReceitaId(token);

		var resposta = await GetRequest($"{METODO}/{receitaId}", token);

		resposta.StatusCode.Should().Be(HttpStatusCode.OK);

		await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responstaBody);

		responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
		responseData.RootElement.GetProperty("titulo").GetString().Should().NotBeNullOrWhiteSpace();
		responseData.RootElement.GetProperty("categoria").GetUInt16().Should().BeInRange(0, 3);
		responseData.RootElement.GetProperty("modoPreparo").GetString().Should().NotBeNullOrWhiteSpace();
		responseData.RootElement.GetProperty("ingredientes").GetArrayLength().Should().BeGreaterThan(0);
		responseData.RootElement.GetProperty("tempoPreparo").GetUInt32().Should().BeGreaterThan(0).And.BeLessThanOrEqualTo(1000);
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

		var erros = responseData.RootElement.GetProperty("mensagens");

		var mensagemEsperada = ResourceMensagensDeErro.ResourceManager.GetString("RECEITA_NAO_ENCONTRADA", new System.Globalization.CultureInfo(cultura));
		erros.Equals(mensagemEsperada);
	}
}


using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Domain.Entidades;
using LivroReceitas.Exceptions;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.V1;

public class ControllerBase : IClassFixture<LivroReceitasWebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public ControllerBase(LivroReceitasWebApplicationFactory<Program> factory)
	{
		_client = factory.CreateClient();
		ResourceMensagensDeErro.Culture = CultureInfo.CurrentCulture;
	}

	protected async Task<HttpResponseMessage> PostRequest(string metodo, object body)
	{
		var jsonString = JsonConvert.SerializeObject(body);

		return await _client.PostAsync(metodo, new StringContent(jsonString, Encoding.UTF8, "application/json"));
	}

	protected async Task<HttpResponseMessage> PutRequest(string metodo, object body, string token = "")
	{
		AutorizarRequisicao(token);
		var jsonString = JsonConvert.SerializeObject(body);


		return await _client.PutAsync(metodo, new StringContent(jsonString, Encoding.UTF8, "application/json"));
	}

	protected async Task<string> Login(string email, string senha)
	{
		var requisicao = new RequisicaoLoginJson
		{
			Email = email,
			Senha = senha
		};
		var resposta = await PostRequest("login", requisicao);

		await using var respostaBody = await resposta.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(respostaBody);


		return responseData.RootElement.GetProperty("token").GetString();
	}

	private void AutorizarRequisicao(string token)
	{
		if (!string.IsNullOrWhiteSpace(token))
		{
			_client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
		}
	}
}

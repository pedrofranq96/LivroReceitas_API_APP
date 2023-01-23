using LivroReceitas.Exceptions;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
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
}

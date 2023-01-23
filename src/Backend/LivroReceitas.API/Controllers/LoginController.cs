using LivroReceitas.Application.UseCases.Login.FazerLogin;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroReceitas.API.Controllers
{
	public class LoginController : LivroReceitasController
	{
		[HttpPost]
		[ProducesResponseType(typeof(RespostaLoginJson), StatusCodes.Status200OK)]
		public async Task<IActionResult> Login([FromServices] ILoginUseCase useCase,[FromBody] RequisicaoLoginJson requisicao)
		{
			var resposta = await useCase.Executar(requisicao);
			return Ok(resposta);
		}
	}
}

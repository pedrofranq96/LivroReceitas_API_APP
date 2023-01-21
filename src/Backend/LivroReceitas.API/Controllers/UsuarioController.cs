using LivroReceitas.Application.UseCases.Usuario.Registrar;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroReceitas.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsuarioController : ControllerBase
	{
	
		[HttpPost]
		[ProducesResponseType(typeof(RespostaUsuarioRegistradoJson),StatusCodes.Status201Created)]
		public async Task<IActionResult> RegistrarUsuario([FromServices]IRegistrarUsuarioUseCase useCase,
			[FromBody] RequisicaoRegistrarUsuarioJson request)
		{
			var resultado = await useCase.Executar(request);

			return Created(string.Empty, resultado);
		}
	}
}
using LivroReceitas.API.Filtros.UsuarioLogado;
using LivroReceitas.Application.UseCases.Usuario.AlterarSenha;
using LivroReceitas.Application.UseCases.Usuario.RecuperarPerfil;
using LivroReceitas.Application.UseCases.Usuario.Registrar;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroReceitas.API.Controllers
{

    public class UsuarioController : LivroReceitasController
	{
	
		[HttpPost]
		[ProducesResponseType(typeof(RespostaUsuarioRegistradoJson),StatusCodes.Status201Created)]
		public async Task<IActionResult> RegistrarUsuario([FromServices]IRegistrarUsuarioUseCase useCase,
			[FromBody] RequisicaoRegistrarUsuarioJson request)
		{
			var resultado = await useCase.Executar(request);

			return Created(string.Empty, resultado);
		}
		
		[HttpPut]
		[Route("alterar-senha")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
		public async Task<IActionResult> AlterarSenha(
			[FromServices]IAlterarSenhaUseCase useCase,
			[FromBody] RequisicaoAlterarSenhaJson request)
		{

			await useCase.Executar(request);
			return NoContent();
		}



		[HttpGet]
		[ProducesResponseType(typeof(RespostaPerfilUsuarioJson), StatusCodes.Status200OK)]
		[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
		public async Task<IActionResult> RecuperarPerfil([FromServices] IRecuperarPerfilUseCase useCase)
		{
			var resultado = await useCase.Executar();
			return Ok(resultado);
		}

	}
}
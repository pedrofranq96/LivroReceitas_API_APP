using LivroReceitas.API.Filtros.UsuarioLogado;
using LivroReceitas.Application.UseCases.Conexao.Recuperar;
using LivroReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroReceitas.API.Controllers;

[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class ConexoesController : LivroReceitasController
{	
	[HttpGet]
	[ProducesResponseType(typeof(IList<RespostaConexoesDoUsuarioJson>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> RecuperarConexoes([FromServices]IRecuperarTodasConexoesUseCase useCase)
	{

		var resultado = await useCase.Executar();

		if (resultado.Usuarios.Any())
		{
			return Ok(resultado);
		}
		return NoContent();
	}

}
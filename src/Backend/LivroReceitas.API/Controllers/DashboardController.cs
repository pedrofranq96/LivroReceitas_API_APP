using LivroReceitas.API.Filtros;
using LivroReceitas.Application.UseCases.DashBoard;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroReceitas.API.Controllers;


public class DashboardController : LivroReceitasController
{	
	[HttpPut]
	[ProducesResponseType(typeof(RespostaDashBoardJson), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
	public async Task<IActionResult> RecuperarDashboard(
		[FromServices]IDashBoardUseCase useCase,
		[FromBody] RequisicaoDashBoardJson request)
	{

		var resultado = await useCase.Executar(request);

		if (resultado.Receitas.Any())
		{
			return Ok(resultado);
		}
		return NoContent();
	}

}
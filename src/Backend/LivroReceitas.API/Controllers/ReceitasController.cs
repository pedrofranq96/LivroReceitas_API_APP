using LivroReceitas.API.Binder;
using LivroReceitas.API.Filtros;
using LivroReceitas.Application.UseCases.Receita.RecuperarPorId;
using LivroReceitas.Application.UseCases.Receita.Registrar;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroReceitas.API.Controllers;


[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class ReceitasController : LivroReceitasController
{

	[HttpPost]
	[ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status201Created)]
	public async Task<IActionResult> Registrar([FromServices] IRegistrarReceitaUseCase useCase,
		[FromBody] RequisicaoRegistrarReceitaJson requisicao)
	{
		var resposta = await useCase.Executar(requisicao);
		return Created(string.Empty, resposta);
	}
	
	[HttpGet]
	[Route("{id:hashids}")]
	[ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status200OK)]
	public async Task<IActionResult> RecuperarPorId([FromServices] IRecuperarReceitaPorIdUseCase useCase,
		[FromRoute] [ModelBinder(typeof(HashidsModelBinder))]long id)
	{
		var resposta = await useCase.Executar(id);
		return Ok(resposta);
	}
}

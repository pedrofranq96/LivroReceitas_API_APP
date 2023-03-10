using LivroReceitas.API.Binder;
using LivroReceitas.API.Filtros.UsuarioLogado;
using LivroReceitas.Application.UseCases.Receita.Atualizar;
using LivroReceitas.Application.UseCases.Receita.Excluir;
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
		[FromBody] RequisicaoReceitaJson requisicao)
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
	
	[HttpPut]
	[Route("{id:hashids}")]
	[ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> AtualizarReceita([FromServices] IReceitaAtualizarUseCase useCase,
		[FromBody] RequisicaoReceitaJson requisicao,
		[FromRoute] [ModelBinder(typeof(HashidsModelBinder))]long id)
	{
		await useCase.Executar(id, requisicao);
		
		return NoContent();
	}
	
	[HttpDelete]
	[Route("{id:hashids}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> DeletarReceita([FromServices] IDeletarReceitaUseCase useCase,
		[FromRoute] [ModelBinder(typeof(HashidsModelBinder))]long id)
	{
		await useCase.Executar(id);
		
		return NoContent();
	}
}

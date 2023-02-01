using AutoMapper;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Domain.Repositorio.Receita;
using LivroReceitas.Exceptions.ExceptionsBase;
using LivroReceitas.Exceptions;
using LivroReceitas.Domain.Repositorio;
using LivroReceitas.Application.UseCases.Receita.Registrar;

namespace LivroReceitas.Application.UseCases.Receita.Atualizar;
public class ReceitaAtualizarUseCase : IReceitaAtualizarUseCase
{
	private readonly IReceitaUpdateOnlyRepositorio _repositorio;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IMapper _mapper;
	private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

	public ReceitaAtualizarUseCase(IReceitaUpdateOnlyRepositorio repositorio,
		IUsuarioLogado usuarioLogado, IMapper mapper, IUnidadeDeTrabalho unidadeDeTrabalho)
	{
		_repositorio = repositorio;
		_usuarioLogado = usuarioLogado;
		_mapper = mapper;
		_unidadeDeTrabalho = unidadeDeTrabalho;
	}
	public async Task Executar(long id, RequisicaoReceitaJson requisicao)
	{

		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		var receita = await _repositorio.RecuperarPorId(id);

		Validar(usuarioLogado,receita, requisicao);
		
		_mapper.Map(requisicao, receita);

		_repositorio.Update(receita);

		await _unidadeDeTrabalho.Commit();
	}

	public static void Validar(Domain.Entidades.Usuario usuarioLogado, Domain.Entidades.Receita receita, RequisicaoReceitaJson requisicao)
	{
		if (receita is null || receita.UsuarioId != usuarioLogado.Id)
		{
			throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA });
		}

		var validator = new RegistrarReceitaValidator();
		var resultado = validator.Validate(requisicao);
		if (!resultado.IsValid)
		{
			var mensagensDeErro = resultado.Errors.Select(c => c.ErrorMessage).ToList();
			throw new ErrosDeValidacaoException(mensagensDeErro);
		}
	}
}

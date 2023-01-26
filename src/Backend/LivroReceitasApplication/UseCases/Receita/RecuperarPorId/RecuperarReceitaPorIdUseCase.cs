using AutoMapper;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Domain.Repositorio.Receita;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;

namespace LivroReceitas.Application.UseCases.Receita.RecuperarPorId;
public class RecuperarReceitaPorIdUseCase : IRecuperarReceitaPorIdUseCase
{
	private readonly IReceitaReadOnlyRepositorio _repositorio;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IMapper _mapper;

	public RecuperarReceitaPorIdUseCase(IReceitaReadOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado, IMapper mapper)
	{
		_repositorio = repositorio;
		_usuarioLogado = usuarioLogado;
		_mapper = mapper;
	}
	public async Task<RespostaReceitaJson> Executar(long id)
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		var receita = await _repositorio.RecuperarPorId(id);
		Validar(usuarioLogado, receita);

		return _mapper.Map<RespostaReceitaJson>(receita);
	}
	public void Validar(Domain.Entidades.Usuario usuarioLogado, Domain.Entidades.Receita receita)
	{
		if (receita == null || receita.UsuarioId != usuarioLogado.Id)
		{			
			throw new ErrosDeValidacaoException(new List<string> {ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA });
		}
	}
}

using AutoMapper;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Domain.Repositorio.Conexao;
using LivroReceitas.Domain.Repositorio.Receita;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;

namespace LivroReceitas.Application.UseCases.Receita.RecuperarPorId;
public class RecuperarReceitaPorIdUseCase : IRecuperarReceitaPorIdUseCase
{
	private readonly IConexaoReadOnlyRepositorio _conexoesRepositorio;
	private readonly IReceitaReadOnlyRepositorio _repositorio;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IMapper _mapper;

	public RecuperarReceitaPorIdUseCase(IConexaoReadOnlyRepositorio conexoesRepositorio, 
		IReceitaReadOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado, IMapper mapper)
	{
		_conexoesRepositorio = conexoesRepositorio;
		_repositorio = repositorio;
		_usuarioLogado = usuarioLogado;
		_mapper = mapper;
	}

	public async Task<RespostaReceitaJson> Executar(long id)
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		var receita = await _repositorio.RecuperarPorId(id);
		await Validar(usuarioLogado, receita);

		return _mapper.Map<RespostaReceitaJson>(receita);
	}
	public async Task Validar(Domain.Entidades.Usuario usuarioLogado, Domain.Entidades.Receita receita)
	{
		var usuariosConectados = await _conexoesRepositorio.RecuperarUsuario(usuarioLogado.Id);
		if (receita is null || (receita.UsuarioId != usuarioLogado.Id && !usuariosConectados.Any(c=>c.Id == receita.UsuarioId)))
		{			
			throw new ErrosDeValidacaoException(new List<string> {ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA });
		}
	}
}

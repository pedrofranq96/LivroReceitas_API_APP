using AutoMapper;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Domain.Repositorio.Conexao;
using LivroReceitas.Domain.Repositorio.Receita;

namespace LivroReceitas.Application.UseCases.Conexao.Recuperar;
public class RecuperarTodasConexoesUseCase : IRecuperarTodasConexoesUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IReceitaReadOnlyRepositorio _repositorioReceita;
	private readonly IConexaoReadOnlyRepositorio _repositorio;
	private readonly IMapper _mapper;

	public RecuperarTodasConexoesUseCase(IUsuarioLogado usuarioLogado, 
		IReceitaReadOnlyRepositorio repositorioReceita, IConexaoReadOnlyRepositorio repositorio, IMapper mapper)
	{
		_usuarioLogado = usuarioLogado;
		_repositorioReceita = repositorioReceita;
		_repositorio = repositorio;
		_mapper = mapper;
	}

	public async Task<IList<RespostaUsuarioConectadoJson>> Executar()
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		var conexoes = await _repositorio.RecuperarUsuario(usuarioLogado.Id);

		var tarefas = conexoes.Select(async usuario=> 
		{
			var quantidadeReceitas = await _repositorioReceita.QuantidadeReceitas(usuario.Id);

			var usuarioJson = _mapper.Map<RespostaUsuarioConectadoJson>(usuario);
			usuarioJson.QuantidadeReceitas = quantidadeReceitas;

			return usuarioJson;
		});

		return await Task.WhenAll(tarefas);

;	}
}

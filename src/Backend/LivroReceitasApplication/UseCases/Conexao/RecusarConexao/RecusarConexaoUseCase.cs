using HashidsNet;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Domain.Repositorio;
using LivroReceitas.Domain.Repositorio.Codigos;

namespace LivroReceitas.Application.UseCases.Conexao.RecusarConexao;
public class RecusarConexaoUseCase : IRecusarConexaoUseCase
{

	private readonly ICodigoWriteOnlyRepositorio _repositorio;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IUnidadeDeTrabalho _unidadeTrabalho;
	private readonly IHashids _hashids;

	public RecusarConexaoUseCase(ICodigoWriteOnlyRepositorio repositorio, 
		IUsuarioLogado usuarioLogado, IUnidadeDeTrabalho unidadeTrabalho, IHashids hashids)
	{
		_repositorio = repositorio;
		_usuarioLogado = usuarioLogado;
		_unidadeTrabalho = unidadeTrabalho;
		_hashids = hashids;
	}

	public async Task<string> Executar()
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		await _repositorio.Deletar(usuarioLogado.Id);

		await _unidadeTrabalho.Commit();

		return _hashids.EncodeLong(usuarioLogado.Id);
	}
}

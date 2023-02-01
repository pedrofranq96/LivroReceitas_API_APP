using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Domain.Repositorio.Conexao;
using LivroReceitas.Exceptions.ExceptionsBase;
using LivroReceitas.Domain.Repositorio;
using LivroReceitas.Exceptions;

namespace LivroReceitas.Application.UseCases.Conexao.RemoverConexao;
public class RemoverConexaoUseCase : IRemoverConexaoUseCase
{
	private readonly IConexaoWriteOnlyRepositorio _repositorioWrite;
	private readonly IConexaoReadOnlyRepositorio _repositorioRead;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

	public RemoverConexaoUseCase(IConexaoWriteOnlyRepositorio repositorioWrite, 
		IConexaoReadOnlyRepositorio repositorioRead, IUsuarioLogado usuarioLogado,
		IUnidadeDeTrabalho unidadeDeTrabalho)
	{
		_repositorioWrite = repositorioWrite;
		_repositorioRead = repositorioRead;
		_usuarioLogado = usuarioLogado;
		_unidadeDeTrabalho = unidadeDeTrabalho;
	}

	public async Task Executar(long idUsuarioConectadoParaRemover)
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		var usuariosConectados = await _repositorioRead.RecuperarUsuario(usuarioLogado.Id);

		Validar(usuariosConectados, idUsuarioConectadoParaRemover);

		await _repositorioWrite.RemoverConexao(usuarioLogado.Id, idUsuarioConectadoParaRemover);

		await _unidadeDeTrabalho.Commit();
	}


	public static void Validar(IList<Domain.Entidades.Usuario> usuariosConectados, long idUsuarioConectadoParaRemover)
	{
		
		if (!usuariosConectados.Any(c => c.Id == idUsuarioConectadoParaRemover))
		{
			throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO });
		}
	}
}

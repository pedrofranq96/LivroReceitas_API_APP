using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Domain.Repositorio.Receita;
using LivroReceitas.Exceptions.ExceptionsBase;
using LivroReceitas.Exceptions;
using LivroReceitas.Domain.Repositorio;

namespace LivroReceitas.Application.UseCases.Receita.Excluir;
public class DeletarReceitaUseCase : IDeletarReceitaUseCase
{
	private readonly IReceitaReadOnlyRepositorio _repositorioRead;
	private readonly IReceitaWriteOnlyRepositorio _repositorioWrite;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

	public DeletarReceitaUseCase(IReceitaReadOnlyRepositorio repositorioRead, 
		IReceitaWriteOnlyRepositorio repositorioWrite,
		IUsuarioLogado usuarioLogado,IUnidadeDeTrabalho unidadeDeTrabalho)
	{
		_repositorioRead = repositorioRead;
		_repositorioWrite = repositorioWrite;
		_usuarioLogado = usuarioLogado;
		_unidadeDeTrabalho = unidadeDeTrabalho;
	}

	public async Task Executar(long id)
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		var receita = await _repositorioRead.RecuperarPorId(id);
		Validar(usuarioLogado, receita);

		await _repositorioWrite.Deletar(id);

		await _unidadeDeTrabalho.Commit();
	}
	public void Validar(Domain.Entidades.Usuario usuarioLogado, Domain.Entidades.Receita receita)
	{
		if (receita == null || receita.UsuarioId != usuarioLogado.Id)
		{
			throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA });
		}
	}
}

using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Domain.Repositorio;
using LivroReceitas.Domain.Repositorio.Codigos;

namespace LivroReceitas.Application.UseCases.Conexao.GerarQRCode;
public class GerarQRCodeUseCase : IGerarQRCodeUseCase
{
	private readonly ICodigoWriteOnlyRepositorio _repositorio;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IUnidadeDeTrabalho _unidadeTrabalho;

	public GerarQRCodeUseCase(ICodigoWriteOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado, IUnidadeDeTrabalho unidadeTrabalho)
	{
		_repositorio = repositorio;
		_usuarioLogado = usuarioLogado;
		_unidadeTrabalho = unidadeTrabalho;
	}
	public async Task<string> Executar()
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		var codigo = new Domain.Entidades.Codigos
		{
			Codigo = Guid.NewGuid().ToString(),
			UsuarioId = usuarioLogado.Id
		};
		await _repositorio.Registrar(codigo);
		await _unidadeTrabalho.Commit();

		return codigo.Codigo;
	}
}

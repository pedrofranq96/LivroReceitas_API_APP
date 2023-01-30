using HashidsNet;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Domain.Repositorio.Codigos;
using LivroReceitas.Domain.Repositorio;
using LivroReceitas.Domain.Repositorio.Conexao;

namespace LivroReceitas.Application.UseCases.Conexao.AceitarConexao;
public class AceitarConexaoUseCase : IAceitarConexaoUseCase
{
	private readonly ICodigoWriteOnlyRepositorio _repositorio;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IUnidadeDeTrabalho _unidadeTrabalho;
	private readonly IHashids _hashids;
	private readonly IConexaoWriteOnlyRepositorio _repositorioConexoes;

	public AceitarConexaoUseCase(ICodigoWriteOnlyRepositorio repositorio,
		IUsuarioLogado usuarioLogado, IUnidadeDeTrabalho unidadeTrabalho,
		IHashids hashids, IConexaoWriteOnlyRepositorio repositorioConexoes)
	{
		_repositorio = repositorio;
		_usuarioLogado = usuarioLogado;
		_unidadeTrabalho = unidadeTrabalho;
		_hashids = hashids;
		_repositorioConexoes = repositorioConexoes;
	}
	public async Task<string> Executar(string usuarioParaSeConectarId)
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		await _repositorio.Deletar(usuarioLogado.Id);

		var idUsuarioLeitorQRCode = _hashids.DecodeLong(usuarioParaSeConectarId).First();

		await _repositorioConexoes.Registrar(new Domain.Entidades.Conexao
		{
			UsuarioId = usuarioLogado.Id,
			ConecatadoComUsuarioId = idUsuarioLeitorQRCode
		});

		await _repositorioConexoes.Registrar(new Domain.Entidades.Conexao
		{
			UsuarioId = idUsuarioLeitorQRCode,
			ConecatadoComUsuarioId = usuarioLogado.Id
		});

		await _unidadeTrabalho.Commit();

		return _hashids.EncodeLong(usuarioLogado.Id);
	}
}

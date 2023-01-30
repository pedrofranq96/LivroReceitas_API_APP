using HashidsNet;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Domain.Repositorio.Codigos;
using LivroReceitas.Domain.Repositorio.Conexao;
using LivroReceitas.Exceptions.ExceptionsBase;

namespace LivroReceitas.Application.UseCases.Conexao.QRCodeLido;
public class QRCodeLidoUseCase : IQRCodeLidoUseCase
{
	private readonly IHashids _hashids;
	private readonly IConexaoReadOnlyRepositorio _repositorioConexao;
	private readonly ICodigoReadOnlyRepositorio _repositorio;
	private readonly IUsuarioLogado _usuarioLogado;

	public QRCodeLidoUseCase(IHashids hashids, IConexaoReadOnlyRepositorio repositorioConexao, 
		ICodigoReadOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado)
	{
		_hashids = hashids;
		_repositorioConexao = repositorioConexao;
		_repositorio = repositorio;
		_usuarioLogado = usuarioLogado;
	}

	public async Task<(RespostaUsuarioConexaoJson usuarioParaSeConectar, string idUsuarioGerouQRCode)> Executar(string codigoConexao)
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
		var codigo = await _repositorio.RecuperarEntidadeCodigo(codigoConexao);

		await Validar(codigo, usuarioLogado);

		var usuarioParaSeConectar = new RespostaUsuarioConexaoJson
		{
			Id = _hashids.EncodeLong(usuarioLogado.Id),
			Nome = usuarioLogado.Nome
		};
		return (usuarioParaSeConectar, _hashids.EncodeLong(codigo.UsuarioId));

	}

	private async Task Validar(Domain.Entidades.Codigos codigo, Domain.Entidades.Usuario usuarioLogado)
	{
		if (codigo is null)
		{
			throw new LivroReceitasException("");
		}

		if (codigo.UsuarioId == usuarioLogado.Id)
		{
			throw new LivroReceitasException("");
		}

		var existeConexao = await _repositorioConexao.ExisteConexao(codigo.UsuarioId, usuarioLogado.Id);

		if (existeConexao)
		{
			throw new LivroReceitasException("");
		}
	}	
}

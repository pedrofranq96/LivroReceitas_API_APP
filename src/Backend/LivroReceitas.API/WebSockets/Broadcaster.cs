using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;


namespace LivroReceitas.API.WebSockets;

public class Broadcaster
{
	private readonly static Lazy<Broadcaster> _instance = new(() => new Broadcaster());

	public static Broadcaster Instance { get { return _instance.Value; } }

	private ConcurrentDictionary<string, object> _dictionary { get; set; }

	public Broadcaster()
	{
		_dictionary = new ConcurrentDictionary<string, object>();
	}
	
	public void InicializarConexao(IHubContext<AdicionarConexao> hubContext,string idUsuarioGerouQRCode, string connectionId)
	{
		var conexao = new Conexao(hubContext, connectionId);

		_dictionary.TryAdd(connectionId, conexao);
		_dictionary.TryAdd(idUsuarioGerouQRCode, connectionId);

		conexao.IniciarContagemTempo(CallBackTempoExpirado);
	}

	private void CallBackTempoExpirado(string connectionId)
	{

		_dictionary.TryRemove(connectionId, out _);
	}

	public string GetConnectionIdDoUsuario(string usuarioId)
	{
		if (!_dictionary.TryGetValue(usuarioId, out var connectionId))
		{
			throw new LivroReceitasException(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO);
		}

		return connectionId.ToString();
	}

	public void ResetarTempoExpiracao(string connectionId)
	{
		_dictionary.TryGetValue(connectionId, out var objetoConexao);

		var conexao = objetoConexao as Conexao;

		conexao.ResetarContagemTempo();
	}


	public void SetConnectionIdUsuarioLeitorQRCode(string idUsuarioGerouQRCode, string connectionIdUsuarioLeitorQRCode)
	{
		var connectionIdUsuarioQueLeuQRCode = GetConnectionIdDoUsuario(idUsuarioGerouQRCode);

		_dictionary.TryGetValue(connectionIdUsuarioQueLeuQRCode, out var objetoConexao);

		var conexao = objetoConexao as Conexao;

		conexao.SetConnectionIdUsuarioLeitorQRCode(connectionIdUsuarioLeitorQRCode);
	}

	public string Remover(string connectionId, string usuarioId)
	{
		if (!_dictionary.TryGetValue(connectionId, out var objetoConexao))
		{
			throw new LivroReceitasException(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO);
		}

		var conexao = objetoConexao as Conexao;

		conexao.StopTimer();

		_dictionary.TryRemove(connectionId, out _);
		_dictionary.TryRemove(usuarioId, out _);

		return conexao.UsuarioQueLeuQRCode();
	}


}

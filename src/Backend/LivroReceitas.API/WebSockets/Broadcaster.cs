using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Linq;

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
	public void InicializarConexao(IHubContext<AdicionarConexao> hubContext, string connectionId)
	{
		var conexao = new Conexao(hubContext, connectionId);

		_dictionary.TryAdd(connectionId, conexao);
		
		conexao.IniciarContagemTempo(CallbackTempoExpirado);
	}

	private void CallbackTempoExpirado(string connectionId)
	{
		_dictionary.TryRemove(connectionId, out _);
	}

	
}

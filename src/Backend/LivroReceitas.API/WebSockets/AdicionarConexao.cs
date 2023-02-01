using LivroReceitas.Application.UseCases.Conexao.AceitarConexao;
using LivroReceitas.Application.UseCases.Conexao.GerarQRCode;
using LivroReceitas.Application.UseCases.Conexao.QRCodeLido;
using LivroReceitas.Application.UseCases.Conexao.RecusarConexao;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LivroReceitas.API.WebSockets;

[Authorize(Policy = "UsuarioLogado")]
public class AdicionarConexao : Hub
{
	private readonly Broadcaster _broadcaster;

	
	private readonly IAceitarConexaoUseCase _aceitarConexaoUseCase;
	private readonly IRecusarConexaoUseCase _recusarConexaoUseCase;
	private readonly IQRCodeLidoUseCase _gerarQRCodelido;
	private readonly IGerarQRCodeUseCase _gerarQRCodeUseCase;
	private readonly IHubContext<AdicionarConexao> _hubContext;
	public AdicionarConexao(IQRCodeLidoUseCase gerarQRCodelido, IRecusarConexaoUseCase recusarConexaoUseCase,
		IHubContext<AdicionarConexao> hubContext,IGerarQRCodeUseCase gerarQRCodeUseCase, IAceitarConexaoUseCase aceitarConexaoUseCase)
	{
		_broadcaster = Broadcaster.Instance;

		_recusarConexaoUseCase = recusarConexaoUseCase;
		_gerarQRCodelido = gerarQRCodelido;
		_hubContext = hubContext;
		_gerarQRCodeUseCase = gerarQRCodeUseCase;
		_aceitarConexaoUseCase = aceitarConexaoUseCase;
	}
	public async Task GetQRCode()
	{	

		try
		{
			(var qrCode, var idUsuario) = await _gerarQRCodeUseCase.Executar();

			_broadcaster.InicializarConexao(_hubContext, idUsuario, Context.ConnectionId);

			await Clients.Caller.SendAsync("ResultadoQRCode", qrCode);
		}
		catch (LivroReceitasException ex)
		{
			await Clients.Caller.SendAsync("Erro", ex.Message);
		}
		catch
		{
			await Clients.Caller.SendAsync("Erro", ResourceMensagensDeErro.ERRO_DESCONHECIDO);
		}
	}
	
	public async Task QRCodeLido(string codigoConexao)
	{
		try
		{
			(var usuarioParaSeConectar, var idUsuarioQueGerouQRCode) = await _gerarQRCodelido.Executar(codigoConexao);

			var connectionId = _broadcaster.GetConnectionIdDoUsuario(idUsuarioQueGerouQRCode);

			_broadcaster.ResetarTempoExpiracao(connectionId);
			_broadcaster.SetConnectionIdUsuarioLeitorQRCode(idUsuarioQueGerouQRCode, Context.ConnectionId);

			await Clients.Client(connectionId).SendAsync("ResultadoQRCodeLido", usuarioParaSeConectar);
		}
		catch (LivroReceitasException ex)
		{
			await Clients.Caller.SendAsync("Erro", ex.Message);
		}
		catch
		{
			await Clients.Caller.SendAsync("Erro", ResourceMensagensDeErro.ERRO_DESCONHECIDO);
		}
	}

	public async Task RecusarConexao()
	{
		try
		{
			var connectionIdUsuarioQueGerouQRCode = Context.ConnectionId;

			var usuarioId = await _recusarConexaoUseCase.Executar();

			var connectionIdUsuarioQueLeuQRCode = _broadcaster.Remover(connectionIdUsuarioQueGerouQRCode, usuarioId);

			await Clients.Client(connectionIdUsuarioQueLeuQRCode).SendAsync("OnConexaoRecusada");
		}
		catch (LivroReceitasException ex)
		{
			await Clients.Caller.SendAsync("Erro", ex.Message);
		}
		catch
		{
			await Clients.Caller.SendAsync("Erro", ResourceMensagensDeErro.ERRO_DESCONHECIDO);
		}
	}

	public async Task AceitarConexao(string idUsuarioParaSeConectar)
	{
		try
		{
			var usuarioId = await _aceitarConexaoUseCase.Executar(idUsuarioParaSeConectar);

			var connectionIdUsuarioQueGerouQRCode = Context.ConnectionId;

			var connectionIdUsuarioQueLeuQRCode = _broadcaster.Remover(connectionIdUsuarioQueGerouQRCode, usuarioId);

			await Clients.Client(connectionIdUsuarioQueLeuQRCode).SendAsync("OnConexaoAceita");
		}
		catch (LivroReceitasException ex)
		{
			await Clients.Caller.SendAsync("Erro", ex.Message);
		}
		catch
		{
			await Clients.Caller.SendAsync("Erro", ResourceMensagensDeErro.ERRO_DESCONHECIDO);
		}
	}
}

using LivroReceitas.Application.UseCases.Conexao.GerarQRCode;
using LivroReceitas.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LivroReceitas.API.WebSockets;

[Authorize(Policy = "UsuarioLogado")]
public class AdicionarConexao : Hub
{
	private readonly IHubContext<AdicionarConexao> _hubContext;
	private readonly Broadcaster _broadcaster;
	private readonly IGerarQRCodeUseCase _gerarQRCodeUseCase;
	public AdicionarConexao(IHubContext<AdicionarConexao> hubContext,IGerarQRCodeUseCase gerarQRCodeUseCase)
	{
		_broadcaster = Broadcaster.Instance;
		_gerarQRCodeUseCase = gerarQRCodeUseCase;
		_hubContext = hubContext;
	}
	
	public async Task GetQRCode()
	{
		var qrCode = await _gerarQRCodeUseCase.Executar();

		_broadcaster.InicializarConexao(_hubContext,Context.ConnectionId);

		await Clients.Caller.SendAsync("ResultadoQRCode", qrCode);
	}
	
}

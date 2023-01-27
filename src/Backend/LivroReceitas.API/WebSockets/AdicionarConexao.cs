﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LivroReceitas.API.WebSockets;

[Authorize(Policy = "UsuarioLogado")]
public class AdicionarConexao : Hub
{
	
	public async Task GetQRCode()
	{
		var qrCode = "ABCD123";

		await Clients.Caller.SendAsync("ResultadoQRCode", qrCode);
	}
	public override Task OnConnectedAsync()
	{
		var x = Context.ConnectionId;
		return base.OnConnectedAsync();
	}
}

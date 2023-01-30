using LivroReceitas.API.WebSockets;
using LivroReceitas.Application.UseCases.Conexao.GerarQRCode;
using LivroReceitas.Application.UseCases.Conexao.RecusarConexao;
using LivroReceitas.Exceptions;
using Moq;
using UtilitarioParaOsTestes.Respostas;
using WebApi.Test.V1.Conexao.Builder;
using Xunit;

namespace WebApi.Test.V1.Conexao;
public class ConexaoRecusadaTeste
{

	[Fact]
	public async Task Validar_Sucesso()
	{
		var codigoGeradoParaConexao = Guid.NewGuid().ToString();
		var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

		(var mockHubContext, var mockClienteProxy, var mockClients, var mockHubContextCaller)
			= MockWebSocketsConnectionClientBuilder.Construir();

		
		var useCaseGerarQRCode = GerarQrCodeUseCaseBuilder(codigoGeradoParaConexao);
		var useCaseConexaoRecusada = GerarConexaoRescusadaUseCaseBuilder();

		var hub = new AdicionarConexao(null, useCaseConexaoRecusada, mockHubContext.Object, useCaseGerarQRCode, null)
		{
			Context = mockHubContextCaller.Object,
			Clients = mockClients.Object,
		};

		await hub.GetQRCode();

		await hub.RecusarConexao();


		mockClienteProxy.Verify(
			clientProxy => clientProxy.SendCoreAsync("OnConexaoRecusada", 
			It.Is<object[]>(resposta => resposta != null && resposta.Length == 0),default), Times.Once);

	}

	[Fact]
	public async Task Validar_ErroDesconhecido()
	{
		var codigoGeradoParaConexao = Guid.NewGuid().ToString();
		var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

		(var mockHubContext, var mockClienteProxy, var mockClients, var mockHubContextCaller)
			= MockWebSocketsConnectionClientBuilder.Construir();

		var useCaseConexaoRecusada = GerarConexaoRecusada_ErroDesconhecido_UseCaseBuilder();
		var useCaseGerarQRCode = GerarQrCodeUseCaseBuilder(codigoGeradoParaConexao);

		var hub = new AdicionarConexao(null, useCaseConexaoRecusada, mockHubContext.Object, useCaseGerarQRCode, null)
		{
			Context = mockHubContextCaller.Object,
			Clients = mockClients.Object,
		};

		await hub.GetQRCode();

		await hub.RecusarConexao();


		mockClienteProxy.Verify(
		clientProxy => clientProxy.SendCoreAsync("Erro", It.Is<object[]>(
			resposta => resposta != null
			&& resposta.Length == 1
			&& resposta.First().Equals(ResourceMensagensDeErro.ERRO_DESCONHECIDO)), default), Times.Once);

	}

	[Fact]
	public async Task Validar_Erro_LivroReceitasException()
	{
		var codigoGeradoParaConexao = Guid.NewGuid().ToString();
		var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

		(var mockHubContext, var mockClienteProxy, var mockClients, var mockHubContextCaller)
			= MockWebSocketsConnectionClientBuilder.Construir();


		var useCaseGerarQRCode = GerarQrCodeUseCaseBuilder(codigoGeradoParaConexao);
		var useCaseConexaoRecusada = GerarConexaoRescusadaUseCaseBuilder();

		var hub = new AdicionarConexao(null, useCaseConexaoRecusada, mockHubContext.Object, useCaseGerarQRCode, null)
		{
			Context = mockHubContextCaller.Object,
			Clients = mockClients.Object,
		};


		await hub.RecusarConexao();


		mockClienteProxy.Verify(
		clientProxy => clientProxy.SendCoreAsync("Erro", It.Is<object[]>(
			resposta => resposta != null
			&& resposta.Length == 1
			&& resposta.First().Equals(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO)), default), Times.Once);

	}
	

	private static IGerarQRCodeUseCase GerarQrCodeUseCaseBuilder(string qrcode)
	{
		var useCaseMock = new Mock<IGerarQRCodeUseCase>();

		useCaseMock.Setup(c => c.Executar()).ReturnsAsync((qrcode, "IdUsuario"));

		return useCaseMock.Object;
	}
	
	private static IRecusarConexaoUseCase GerarConexaoRescusadaUseCaseBuilder()
	{
		var useCaseMock = new Mock<IRecusarConexaoUseCase>();

		useCaseMock.Setup(c => c.Executar()).ReturnsAsync("IdUsuario");

		return useCaseMock.Object;
	}

	private static IRecusarConexaoUseCase GerarConexaoRecusada_ErroDesconhecido_UseCaseBuilder()
	{
		var useCaseMock = new Mock<IRecusarConexaoUseCase>();

		useCaseMock.Setup(c => c.Executar()).ThrowsAsync(new ArgumentOutOfRangeException());
		return useCaseMock.Object;
	}
}

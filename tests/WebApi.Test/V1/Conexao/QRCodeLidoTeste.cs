using LivroReceitas.API.WebSockets;
using LivroReceitas.Application.UseCases.Conexao.GerarQRCode;
using LivroReceitas.Application.UseCases.Conexao.QRCodeLido;
using LivroReceitas.Comunicacao.Respostas;
using Moq;
using WebApi.Test.V1.Conexao.Builder;
using Xunit;
using UtilitarioParaOsTestes.Respostas;
using LivroReceitas.Exceptions;

namespace WebApi.Test.V1.Conexao;
public class QRCodeLidoTeste
{

	[Fact]
	public async Task Validar_Sucesso()
	{
		var codigoGeradoParaConexao = Guid.NewGuid().ToString();
		var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

		(var mockHubContext, var mockClienteProxy, var mockClients, var mockHubContextCaller)
			= MockWebSocketsConnectionClientBuilder.Construir();

		var useCaseQRCodeLido = QRCodeLidoUseCaseBuilder(usuarioParaSeConectar, codigoGeradoParaConexao);
		var useCaseGerarQRCode = GerarQrCodeUseCaseBuilder(codigoGeradoParaConexao);

		var hub = new AdicionarConexao(useCaseQRCodeLido, null, mockHubContext.Object, useCaseGerarQRCode, null)
		{
			Context = mockHubContextCaller.Object,
			Clients = mockClients.Object,
		};

		await hub.GetQRCode();

		await hub.QRCodeLido(codigoGeradoParaConexao);


		mockClienteProxy.Verify(
			clientProxy => clientProxy.SendCoreAsync("ResultadoQRCodeLido", It.Is<object[]>(resposta => resposta != null
				&& resposta.Length == 1
				&& (resposta.First() as RespostaUsuarioConexaoJson).Nome.Equals(usuarioParaSeConectar.Nome)
				&& (resposta.First() as RespostaUsuarioConexaoJson).Id.Equals(usuarioParaSeConectar.Id)), default), Times.Once);

	}
	
	[Fact]
	public async Task Validar_ErroDesconhecido()
	{
		var codigoGeradoParaConexao = Guid.NewGuid().ToString();
		var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

		(var mockHubContext, var mockClienteProxy, var mockClients, var mockHubContextCaller)
			= MockWebSocketsConnectionClientBuilder.Construir();

		var useCaseQRCodeLido = QRCodeLidoUseCase_ErroDesconhecidoBuilder(usuarioParaSeConectar, codigoGeradoParaConexao);
		var useCaseGerarQRCode = GerarQrCodeUseCaseBuilder(codigoGeradoParaConexao);

		var hub = new AdicionarConexao(useCaseQRCodeLido, null, mockHubContext.Object, useCaseGerarQRCode, null)
		{
			Context = mockHubContextCaller.Object,
			Clients = mockClients.Object,
		};

		await hub.GetQRCode();

		await hub.QRCodeLido(codigoGeradoParaConexao);


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

		var useCaseQRCodeLido = QRCodeLidoUseCaseBuilder(usuarioParaSeConectar, codigoGeradoParaConexao);
	
		var hub = new AdicionarConexao(useCaseQRCodeLido,null, mockHubContext.Object, null, null)
		{
			Context = mockHubContextCaller.Object,
			Clients = mockClients.Object,
		};


		await hub.QRCodeLido(codigoGeradoParaConexao);


		mockClienteProxy.Verify(
		clientProxy => clientProxy.SendCoreAsync("Erro", It.Is<object[]>(
			resposta => resposta != null
			&& resposta.Length == 1
			&& resposta.First().Equals(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO)), default), Times.Once);

	}




	private static IQRCodeLidoUseCase QRCodeLidoUseCaseBuilder(RespostaUsuarioConexaoJson respostaJson,string qrcode)
	{
		var useCaseMock = new Mock<IQRCodeLidoUseCase>();

		useCaseMock.Setup(c => c.Executar(qrcode)).ReturnsAsync((respostaJson, "IdUsuario"));

		return useCaseMock.Object;
	}

	private static IQRCodeLidoUseCase QRCodeLidoUseCase_ErroDesconhecidoBuilder(RespostaUsuarioConexaoJson respostaJson, string qrcode)
	{
		var useCaseMock = new Mock<IQRCodeLidoUseCase>();

		useCaseMock.Setup(c => c.Executar(qrcode)).ThrowsAsync(new ArgumentNullException());

		return useCaseMock.Object;
	}


	private static IGerarQRCodeUseCase GerarQrCodeUseCaseBuilder(string qrcode)
	{
		var useCaseMock = new Mock<IGerarQRCodeUseCase>();

		useCaseMock.Setup(c => c.Executar()).ReturnsAsync((qrcode, "IdUsuario"));

		return useCaseMock.Object;
	}
}

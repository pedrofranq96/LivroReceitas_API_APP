using LivroReceitas.API.WebSockets;
using LivroReceitas.Application.UseCases.Conexao.AceitarConexao;
using LivroReceitas.Application.UseCases.Conexao.GerarQRCode;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;
using Moq;
using UtilitarioParaOsTestes.Image;
using UtilitarioParaOsTestes.Respostas;
using WebApi.Test.V1.Conexao.Builder;
using Xunit;

namespace WebApi.Test.V1.Conexao;
public class GetQRCodeTeste
{
	[Fact]
	public async Task Validar_Sucesso()
	{
		var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

		(var mockHubContext, var mockClienteProxy, var mockClients, var mockHubContextCaller) = MockWebSocketsConnectionClientBuilder.Construir();

		var useCaseGerarQRCode = GerarQRCodeUseCaseBuilder();
		var useCaseConexaoAceita = GerarConexaoAceitaUseCaseBuilder(usuarioParaSeConectar.Id);

		var hub = new AdicionarConexao(null, null, mockHubContext.Object, useCaseGerarQRCode, useCaseConexaoAceita)
		{
			Context = mockHubContextCaller.Object,
			Clients = mockClients.Object,
		};

		await hub.GetQRCode();
		await hub.AceitarConexao(usuarioParaSeConectar.Id);

		mockClienteProxy.Verify(
			clientProxy => clientProxy.SendCoreAsync("OnConexaoAceita",
			It.Is<object[]>(resposta => resposta != null && resposta.Length == 0), default), Times.Once);

	}


	[Fact]
	public async Task Validar_Erro_Desconhecido()
	{
		var codigoGeradoParaConexao = Guid.NewGuid().ToString();

		(var mockHubContext, var mockClienteProxy, var mockClients, var mockHubContextCaller) = MockWebSocketsConnectionClientBuilder.Construir();

		var useCaseGerarQRCode = GerarQrCodeUseCaseErro_DesconhecidoBuilder();


		var hub = new AdicionarConexao(null, null, mockHubContext.Object, useCaseGerarQRCode, null)
		{
			Context = mockHubContextCaller.Object,
			Clients = mockClients.Object,
		};

		await hub.GetQRCode();


		mockClienteProxy.Verify(
			clientProxy => clientProxy.SendCoreAsync("Erro", It.Is<object[]>(
				resposta => resposta != null
				&& resposta.Length == 1 && resposta.First().Equals(ResourceMensagensDeErro.ERRO_DESCONHECIDO)), default), Times.Once);

	}

	[Fact]
	public async Task Validar_Erro_LivroReceitas()
	{
		var codigoGeradoParaConexao = Guid.NewGuid().ToString();

		(var mockHubContext, var mockClienteProxy, var mockClients, var mockHubContextCaller) 
			= MockWebSocketsConnectionClientBuilder.Construir();

		var useCaseGerarQRCode = GerarQrCodeUseCaseErro_ExceptionLivroReceitas(ResourceMensagensDeErro.VOCE_NAO_PODE_EXECUTAR_ESTA_OPERACAO);


		var hub = new AdicionarConexao(null, null, mockHubContext.Object, useCaseGerarQRCode, null)
		{
			Context = mockHubContextCaller.Object,
			Clients = mockClients.Object,
		};

		await hub.GetQRCode();


		mockClienteProxy.Verify(
			clientProxy => clientProxy.SendCoreAsync("Erro", It.Is<object[]>(
				resposta => resposta != null
				&& resposta.Length == 1 && resposta.First().Equals(
					ResourceMensagensDeErro.VOCE_NAO_PODE_EXECUTAR_ESTA_OPERACAO)), default), Times.Once);

	}

	


	private static IGerarQRCodeUseCase GerarQrCodeUseCaseErro_DesconhecidoBuilder()
	{
		var useCaseMock = new Mock<IGerarQRCodeUseCase>();

		useCaseMock.Setup(c => c.Executar()).ThrowsAsync(new ArgumentNullException());

		return useCaseMock.Object;
	}

	private static IGerarQRCodeUseCase GerarQrCodeUseCaseErro_ExceptionLivroReceitas(string mensagemErro)
	{
		var useCaseMock = new Mock<IGerarQRCodeUseCase>();

		useCaseMock.Setup(c => c.Executar()).ThrowsAsync(new LivroReceitasException(mensagemErro));

		return useCaseMock.Object;
	}

	private static IAceitarConexaoUseCase GerarConexaoAceitaUseCaseBuilder(string idUsuarioParaSeConectar)
	{
		var useCaseMock = new Mock<IAceitarConexaoUseCase>();

		useCaseMock.Setup(c => c.Executar(idUsuarioParaSeConectar)).ReturnsAsync("IdUsuario");

		return useCaseMock.Object;
	}

	private static IGerarQRCodeUseCase GerarQRCodeUseCaseBuilder()
	{
		var useCaseMock = new Mock<IGerarQRCodeUseCase>();

		useCaseMock.Setup(c => c.Executar()).ReturnsAsync((ImageBase64Builder.Construir(), "IdUsuario"));

		return useCaseMock.Object;
	}

}

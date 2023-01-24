using FluentAssertions;
using LivroReceitas.Application.UseCases.Usuario.Registrar;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;
using UtilitarioParaOsTestes.Criptografia;
using UtilitarioParaOsTestes.Mapper;
using UtilitarioParaOsTestes.Repositorios;
using UtilitarioParaOsTestes.Requisicoes;
using UtilitarioParaOsTestes.Token;
using Xunit;

namespace UseCase.Test.Usuario.Registrar;

public class RegistrarUsuarioUseCaseTeste
{
	[Fact]
	public async Task Validar_Sucesso()
	{
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
		var useCase = CriarUseCase();

		var resposta = await useCase.Executar(requisicao);

		resposta.Should().NotBeNull();
		resposta.Token.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Validar_Erro_EmailRegistrado()
	{
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
		var useCase = CriarUseCase(requisicao.Email);
		Func<Task> acao = async () => { await useCase.Executar(requisicao); };
		

		await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
			.Where(exception => exception.MensagensDeErro.Count == 1 
			&& exception.MensagensDeErro.Contains(ResourceMensagensDeErro.EMAIL_JA_REGISTRADO));
	}
	
	[Fact]
	public async Task Validar_Erro_Email_Vazio()
	{
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
		requisicao.Email = string.Empty;
		var useCase = CriarUseCase();
		Func<Task> acao = async () => { await useCase.Executar(requisicao); };
		

		await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
			.Where(exception => exception.MensagensDeErro.Count == 1 
			&& exception.MensagensDeErro.Contains(ResourceMensagensDeErro.EMAIL_USUARIO_EMBRANCO));
	}


	private static RegistrarUsuarioUseCase CriarUseCase(string email = "")
	{
		var mapper = MapperBuilder.Instancia();
		var repositorio = UsuarioWriteOnlyRepositorioBuilder.Intancia().Construir();
		var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Intancia().Construir();
		var encriptador = EncriptadorDeSenhaBuilder.Instancia();
		var token = TokenControllerBuilder.Instancia();
		var repositoruioReadOnly = UsuarioReadOnlyRepositorioBuilder.Intancia().ExiteUsuarioComEmail(email).Construir();

		return new RegistrarUsuarioUseCase(repositoruioReadOnly,repositorio, mapper, unidadeDeTrabalho, encriptador, token);
	}
}

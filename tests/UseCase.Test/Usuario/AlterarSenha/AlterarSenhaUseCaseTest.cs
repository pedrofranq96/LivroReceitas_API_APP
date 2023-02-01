using FluentAssertions;
using LivroReceitas.Application.UseCases.Usuario.AlterarSenha;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;
using UtilitarioParaOsTestes.Criptografia;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.Repositorios;
using UtilitarioParaOsTestes.Requisicoes;
using UtilitarioParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCase.Test.Usuario.AlterarSenha;
public class AlterarSenhaUseCaseTest
{
	[Fact]
	public async Task Validar_Sucesso()
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();
		var useCase = CriarUseCase(usuario);

		var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
		requisicao.SenhaAtual = senha;

		Func<Task> acao = async () =>
		{
			await useCase.Executar(requisicao);
		};
		await acao.Should().NotThrowAsync();

	}
	
	[Fact]
	public async Task Validar_Erro_NovaSenha_EmBranco()
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();
		var useCase = CriarUseCase(usuario);


		Func<Task> acao = async () =>
		{
			await useCase.Executar(new RequisicaoAlterarSenhaJson
			{
				SenhaAtual = senha,
				NovaSenha = ""
			});
		};
		await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
			.Where(ex=> ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));

	}
	[Fact]
	public async Task Validar_Erro_Senha_Atual_Invalida()
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();
		var useCase = CriarUseCase(usuario);

		var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
		requisicao.SenhaAtual = "senhaInvalida";


		Func<Task> acao = async () =>
		{
			await useCase.Executar(requisicao);
		};
		await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
			.Where(ex=> ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA));

	}
	
	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	public async Task Validar_Erro_SenhaAtual_Minimo_Caracteres(int tamanhoSenha)
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();
		var useCase = CriarUseCase(usuario);

		var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir(tamanhoSenha);
		requisicao.SenhaAtual = senha;

		Func<Task> acao = async () =>
		{
			await useCase.Executar(requisicao);
		};
		await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
			.Where(ex=> ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES));

	}

	private static AlterarSenhaUseCase CriarUseCase(LivroReceitas.Domain.Entidades.Usuario usuario)
	{
		var encriptador = EncriptadorDeSenhaBuilder.Instancia();
		var unidadeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
		var repositorio = UsuarioUpdateOnlyRepositorioBuilder.Instancia().RecuperarPorId(usuario).Construir();
		var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
		return new AlterarSenhaUseCase(usuarioLogado, repositorio, encriptador, unidadeTrabalho);
	}
}

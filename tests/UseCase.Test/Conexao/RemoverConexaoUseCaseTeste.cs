using FluentAssertions;
using LivroReceitas.Application.UseCases.Conexao.RemoverConexao;
using LivroReceitas.Exceptions.ExceptionsBase;
using LivroReceitas.Exceptions;
using UseCase.Test.Conexao.InlineData;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.Repositorios;
using UtilitarioParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCase.Test.Conexao;
public class RemoverConexaoUseCaseTeste
{
	[Theory]
	[ClassData(typeof(EntidadesUsuarioConexaoDataTeste))]
	public async Task Validar_Sucesso(long usuarioIdParaRemover, IList<LivroReceitas.Domain.Entidades.Usuario> conexoes)
	{
		(var usuario, var _) = UsuarioBuilder.Construir();

		var useCase = CriarUseCase(usuario, conexoes);
		

		Func<Task> acao = async () => { await useCase.Executar(usuarioIdParaRemover); };

		await acao.Should().NotThrowAsync();

	}

	[Fact]
	public async Task Validar_Erro_UsuarioInvalido()
	{
		(var usuario, var _) = UsuarioBuilder.Construir();

		var conexoes = ConexaoBuilder.Construir();

		var useCase = CriarUseCase(usuario, conexoes);

		Func<Task> acao = async () => { await useCase.Executar(0); };

		await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
			.Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO));
	}



	private static RemoverConexaoUseCase CriarUseCase(
		LivroReceitas.Domain.Entidades.Usuario usuario,
		IList<LivroReceitas.Domain.Entidades.Usuario> conexoes)
	{
		var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
		var repositorioRead = ConexaoReadOnlyRepositorioBuilder.Instancia().RecuperarDoUsuario(usuario, conexoes).Construir();
		var repositorioWrite = ConexaoWriteOnlyRepositorioBuilder.Instancia().Construir();
		var unidadeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
		return new RemoverConexaoUseCase(repositorioWrite, repositorioRead,usuarioLogado, unidadeTrabalho);
	}
}

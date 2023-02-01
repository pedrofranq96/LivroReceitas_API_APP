using FluentAssertions;
using LivroReceitas.Application.UseCases.Receita.Excluir;
using LivroReceitas.Exceptions.ExceptionsBase;
using LivroReceitas.Exceptions;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.Repositorios;
using UtilitarioParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCase.Test.Receita.Deletar;
public class DeletarReceitaUseCaseTeste
{
	[Fact]
	public async Task Validar_Sucesso()
	{
		(var usuario, var _) = UsuarioBuilder.Construir();

		var receita = ReceitaBuilder.Construir(usuario);

		var useCase = CriarUseCase(usuario, receita);

		Func<Task> acao = async () => { await useCase.Executar(receita.Id); };

		await acao.Should().NotThrowAsync();
	}

	[Fact]
	public async Task Validar_Erro_Receita_Nao_Existe()
	{
		(var usuario, var _) = UsuarioBuilder.Construir();

		var receita = ReceitaBuilder.Construir(usuario);

		var useCase = CriarUseCase(usuario, receita);

		Func<Task> acao = async () => { await useCase.Executar(0); };

		await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
			.Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
	}

	[Fact]
	public async Task Validar_Erro_Receita_Nao_Pertence_Usuario_Logado()
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();
		(var usuario2, _) = UsuarioBuilder.ConstruirUsuario2();

		var receita = ReceitaBuilder.Construir(usuario2);

		var useCase = CriarUseCase(usuario, receita);

		Func<Task> acao = async () => { await useCase.Executar(receita.Id); };

		await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
			.Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
	}

	private static DeletarReceitaUseCase CriarUseCase(LivroReceitas.Domain.Entidades.Usuario usuario, LivroReceitas.Domain.Entidades.Receita receita)
	{
		var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
		var repositorioWrite = ReceitaWriteOnlyRepositorioBuilder.Instancia().Construir();
		var repositorioRead = ReceitaReadOnlyRepositorioBuilder.Instancia().RecuperarPorId(receita).Construir();
		var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();

		return new DeletarReceitaUseCase(repositorioRead, repositorioWrite, usuarioLogado, unidadeDeTrabalho);
	}
}

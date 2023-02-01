using FluentAssertions;
using LivroReceitas.Application.UseCases.DashBoard;
using LivroReceitas.Comunicacao.Requesicoes;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.Mapper;
using UtilitarioParaOsTestes.Repositorios;
using UtilitarioParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCase.Test.Dashboard;
public class DashboardUseCaseTest
{
	[Fact]
	public async Task Validar_Sucesso_Sem_Receitas()
	{
		(var usuario, var _) = UsuarioBuilder.ConstruirUsuario2();
		var conexoes = ConexaoBuilder.Construir();

		var useCase = CriarUseCase(usuario, conexoes);

		var resposta = await useCase.Executar(new());

		resposta.Receitas.Should().HaveCount(0);
	}

	[Fact]
	public async Task Validar_Sucesso_Sem_Filtro()
	{
		(var usuario, var _) = UsuarioBuilder.Construir();
		var conexoes = ConexaoBuilder.Construir();

		var receita = ReceitaBuilder.Construir(usuario);

		var useCase = CriarUseCase(usuario, conexoes, receita);

		var resposta = await useCase.Executar(new());

		resposta.Receitas.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async Task Validar_Sucesso_Filtro_Titulo()
	{
		(var usuario, var _) = UsuarioBuilder.Construir();
		var conexoes = ConexaoBuilder.Construir();

		var receita = ReceitaBuilder.Construir(usuario);

		var useCase = CriarUseCase(usuario, conexoes, receita);

		var resposta = await useCase.Executar(new RequisicaoDashBoardJson
		{
			TituloIngrediente = receita.Titulo.ToUpper()
		});

		resposta.Receitas.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async Task Validar_Sucesso_Filtro_Ingredientes()
	{
		(var usuario, var _) = UsuarioBuilder.Construir();
		var conexoes = ConexaoBuilder.Construir();

		var receita = ReceitaBuilder.Construir(usuario);

		var useCase = CriarUseCase(usuario, conexoes, receita);

		var resposta = await useCase.Executar(new RequisicaoDashBoardJson
		{
			TituloIngrediente = receita.Ingredientes.First().Produto.ToUpper()
		});

		resposta.Receitas.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async Task Validar_Sucesso_Filtro_Categoria()
	{
		(var usuario, var _) = UsuarioBuilder.Construir();
		var conexoes = ConexaoBuilder.Construir();

		var receita = ReceitaBuilder.Construir(usuario);

		var useCase = CriarUseCase(usuario, conexoes, receita);

		var resposta = await useCase.Executar(new RequisicaoDashBoardJson
		{
			Categoria = (LivroReceitas.Comunicacao.Enum.Categoria)receita.Categoria
		});

		resposta.Receitas.Should().HaveCountGreaterThan(0);
	}

	private static DashBoardUseCase CriarUseCase(
		LivroReceitas.Domain.Entidades.Usuario usuario,
		IList<LivroReceitas.Domain.Entidades.Usuario> usuariosConectados,
		LivroReceitas.Domain.Entidades.Receita? receita = null)
	{
		var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
		var mapper = MapperBuilder.Instancia();
		var repositorioRead = ReceitaReadOnlyRepositorioBuilder.Instancia().RecuperarTodasDoUsuario(receita).Construir();
		var repositorioConexao = ConexaoReadOnlyRepositorioBuilder.Instancia().RecuperarDoUsuario(usuario, usuariosConectados).Construir();

		return new DashBoardUseCase(repositorioRead, repositorioConexao, usuarioLogado, mapper);
	}
}

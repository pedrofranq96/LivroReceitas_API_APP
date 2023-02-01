using FluentAssertions;
using LivroReceitas.Application.UseCases.Receita.Registrar;
using LivroReceitas.Exceptions.ExceptionsBase;
using LivroReceitas.Exceptions;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.Mapper;
using UtilitarioParaOsTestes.Repositorios;
using UtilitarioParaOsTestes.Requisicoes;
using UtilitarioParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCase.Test.Receita.Registrar;
public class RegistrarReceitaUseCaseTeste
{
	[Fact]
	public async Task Validar_Sucesso()
	{
		(var usuario, var _) = UsuarioBuilder.Construir();

		var useCase = CriarUseCase(usuario);

		var requisicao = RequisicaoReceitaBuilder.Construir();

		var resposta = await useCase.Executar(requisicao);

		resposta.Should().NotBeNull();

		resposta.Id.Should().NotBeNullOrWhiteSpace();
		resposta.Titulo.Should().Be(requisicao.Titulo);
		resposta.Categoria.Should().Be(requisicao.Categoria);
		resposta.ModoPreparo.Should().Be(requisicao.ModoPreparo);
	
	}

	[Fact]
	public async Task Validar_Erro_Email_Vazio()
	{
		(var usuario, var senha) = UsuarioBuilder.Construir();

		var useCase = CriarUseCase(usuario);

		var requisicao = RequisicaoReceitaBuilder.Construir();
		requisicao.Ingredientes.Clear();

		Func<Task> acao = async () => { await useCase.Executar(requisicao); };

		await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
			.Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_MINIMO_UM_INGREDIENTE));
	}


	private static RegistrarReceitaUseCase CriarUseCase(LivroReceitas.Domain.Entidades.Usuario usuario)
	{
		var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
		var mapper = MapperBuilder.Instancia();
		var repositorio = ReceitaWriteOnlyRepositorioBuilder.Instancia().Construir();
		var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();

		return new RegistrarReceitaUseCase(mapper, unidadeDeTrabalho, usuarioLogado, repositorio);
	}
}

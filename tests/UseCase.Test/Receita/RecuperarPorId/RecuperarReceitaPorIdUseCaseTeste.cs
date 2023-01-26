using FluentAssertions;
using LivroReceitas.Application.UseCases.Receita.RecuperarPorId;
using LivroReceitas.Exceptions.ExceptionsBase;
using LivroReceitas.Exceptions;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.UsuarioLogado;
using Xunit;
using Utilitario.ParaOsTestes.Mapper;

namespace UseCase.Test.Receita.RecuperarPorId;
public class RecuperarReceitaPorIdUseCaseTeste
{
	//[Fact]
	//public async Task Validar_Sucesso()
	//{
	//	(var usuario, var _) = UsuarioBuilder.Construir();
	//	var conexoes = ConexaoBuilder.Construir();

	//	var receita = ReceitaBuilder.Construir(usuario);

	//	var useCase = CriarUseCase(usuario, conexoes, receita);

	//	var resposta = await useCase.Executar(receita.Id);

	//	resposta.Titulo.Should().Be(receita.Titulo);
	//	resposta.Categoria.Should().Be((LivroReceitas.Comunicacao.Enum.Categoria)receita.Categoria);
	//	resposta.ModoPreparo.Should().Be(receita.ModoPreparo);
	//	resposta.TempoPreparo.Should().Be(receita.TempoPreparo);
	//	resposta.Ingredientes.Should().HaveCount(receita.Ingredientes.Count);
	//}

	//[Fact]
	//public async Task Validar_Erro_Receita_Nao_Existe()
	//{
	//	(var usuario, var _) = UsuarioBuilder.Construir();
	//	var conexoes = ConexaoBuilder.Construir();

	//	var receita = ReceitaBuilder.Construir(usuario);

	//	var useCase = CriarUseCase(usuario, conexoes, receita);

	//	Func<Task> acao = async () => { await useCase.Executar(0); };

	//	await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
	//		.Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
	//}

	//[Fact]
	//public async Task Validar_Erro_Receita_Nao_Pertence_Usuario_Logado()
	//{
	//	(var usuario, var senha) = UsuarioBuilder.Construir();
	//	(var usuario2, _) = UsuarioBuilder.ConstruirUsuario2();
	//	var conexoes = ConexaoBuilder.Construir();

	//	var receita = ReceitaBuilder.Construir(usuario2);

	//	var useCase = CriarUseCase(usuario, conexoes, receita);

	//	Func<Task> acao = async () => { await useCase.Executar(receita.Id); };

	//	await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
	//		.Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
	//}

	//private static RecuperarReceitaPorIdUseCase CriarUseCase(
	//	LivroReceitas.Domain.Entidades.Usuario usuario,
	//	IList<LivroReceitas.Domain.Entidades.Usuario> usuariosConectados,
	//	LivroReceitas.Domain.Entidades.Receita receita)
	//{
	//	var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
	//	var mapper = MapperBuilder.Instancia();
	//	var repositorioRead = ReceitaReadOnlyRepositorioBuilder.Instancia().RecuperarPorId(receita).Construir();
	//	var repositorioConexao = ConexaoReadOnlyRepositorioBuilder.Instancia().RecuperarDoUsuario(usuario, usuariosConectados).Construir();

	//	//return new RecuperarReceitaPorIdUseCase(repositorioRead, usuarioLogado, mapper, repositorioConexao);
	//}
}

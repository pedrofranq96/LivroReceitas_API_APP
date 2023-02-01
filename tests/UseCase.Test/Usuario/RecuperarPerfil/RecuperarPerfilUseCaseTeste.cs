using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.Mapper;
using UtilitarioParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCase.Test.Usuario.RecuperarPerfil;
public class RecuperarPerfilUseCaseTeste
{
	//[Fact]
	//public async Task Validar_Sucesso()
	//{
	//	(var usuario, _) = UsuarioBuilder.Construir();

	//	var useCase = CriarUseCase(usuario);

	//	var resposta = await useCase.Executar();

	//	resposta.Should().NotBeNull();
	//	resposta.Nome.Should().Be(usuario.Nome);
	//	resposta.Email.Should().Be(usuario.Email);
	//	resposta.Telefone.Should().Be(usuario.Telefone);
	//}

	//private static RecuperarPerfilUseCase CriarUseCase(LivroReceitas.Domain.Entidades.Usuario usuario)
	//{
	//	var mapper = MapperBuilder.Instancia();
	//	var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();

	//	return new RecuperarPerfilUseCase(mapper, usuarioLogado);
	//}
}

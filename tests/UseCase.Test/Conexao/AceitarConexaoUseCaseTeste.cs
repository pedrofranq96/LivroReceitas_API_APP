﻿using FluentAssertions;
using LivroReceitas.Application.UseCases.Conexao.AceitarConexao;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.HashIds;
using UtilitarioParaOsTestes.Repositorios;
using UtilitarioParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCase.Test.Conexao;
public class AceitarConexaoUseCaseTeste
{
	[Fact]
	public async Task Validar_Sucesso()
	{
		(var usuario, var _) = UsuarioBuilder.Construir();
		(var usuarioParaSeConectar, var _) = UsuarioBuilder.ConstruirUsuario2();

		var useCase = CriarUseCase(usuario);

		var hashids = HashidsBuilder.Instance().Build();
		var resultado = await useCase.Executar(hashids.EncodeLong(usuarioParaSeConectar.Id));

		resultado.Should().NotBeNullOrWhiteSpace();
		resultado.Should().Be(hashids.EncodeLong(usuario.Id));
	}

	private static AceitarConexaoUseCase CriarUseCase(LivroReceitas.Domain.Entidades.Usuario usuario)
	{
		var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
		var repositorioWrite = CodigoWriteOnlyRepositorioBuilder.Instancia().Construir();
		var conexaoWrite = ConexaoWriteOnlyRepositorioBuilder.Instancia().Construir();
		var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
		var hashids = HashidsBuilder.Instance().Build();

		return new AceitarConexaoUseCase(repositorioWrite, usuarioLogado, unidadeDeTrabalho, hashids, conexaoWrite);
	}
}

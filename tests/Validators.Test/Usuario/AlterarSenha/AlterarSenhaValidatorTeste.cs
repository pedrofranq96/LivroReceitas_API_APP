﻿using FluentAssertions;
using LivroReceitas.Application.UseCases.Usuario.AlterarSenha;
using LivroReceitas.Application.UseCases.Usuario.Registrar;
using LivroReceitas.Exceptions;
using UtilitarioParaOsTestes.Requisicoes;
using Xunit;

namespace Validators.Test.Usuario.AlterarSenha;
public class AlterarSenhaValidatorTeste
{
	[Fact]
	public void Validar_Sucesso()
	{
		var validator = new AlterarSenhaValidator();

		var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();

		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeTrue();

	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	public void Validar_Erro_Senha_Invalida(int tamanhoSenha)
	{
		var validator = new AlterarSenhaValidator();

		var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir(tamanhoSenha);
		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeFalse();
		resultado.Errors.Should().ContainSingle().And
			.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES));
	}

	[Fact]
	public void Validar_Erro_Senha_Vazio()
	{
		var validator = new AlterarSenhaValidator();

		var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
		requisicao.NovaSenha = string.Empty;
		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeFalse();
		resultado.Errors.Should().ContainSingle().And
			.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
	}
}

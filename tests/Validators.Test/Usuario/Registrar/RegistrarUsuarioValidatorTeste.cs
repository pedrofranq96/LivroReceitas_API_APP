using FluentAssertions;
using LivroReceitas.Application.UseCases.Usuario.Registrar;
using LivroReceitas.Exceptions;
using UtilitarioParaOsTestes.Requisicoes;
using Xunit;

namespace Validators.Test.Usuario.Registrar;

public class RegistrarUsuarioValidatorTeste
{
	[Fact]
	public void Validar_Sucesso()
	{
		var validator = new RegistrarUsuarioValidator();

		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();

		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeTrue();
		
	}
	[Fact]
	public  void Validar_Erro_Nome_Vazio()
	{
		var validator = new RegistrarUsuarioValidator();
		
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
		requisicao.Nome = string.Empty;
		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeFalse();
		resultado.Errors.Should().ContainSingle().And
			.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO));
	}


	[Fact]
	public  void Validar_Erro_Email_Vazio()
	{
		var validator = new RegistrarUsuarioValidator();
		
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
		requisicao.Email = string.Empty;
		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeFalse();
		resultado.Errors.Should().ContainSingle().And
			.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_EMBRANCO));
	}	
	
	[Fact]
	public  void Validar_Erro_Email_Invalido()
	{
		var validator = new RegistrarUsuarioValidator();
		
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
		requisicao.Email = "we";
		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeFalse();
		resultado.Errors.Should().ContainSingle().And
			.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_INVALIDO));
	}
	
	[Fact]
	public  void Validar_Erro_Senha_Vazio()
	{
		var validator = new RegistrarUsuarioValidator();
		
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
		requisicao.Senha = string.Empty;
		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeFalse();
		resultado.Errors.Should().ContainSingle().And
			.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
	}
	
	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	public  void Validar_Erro_Senha_Invalida(int tamanhoSenha)
	{
		var validator = new RegistrarUsuarioValidator();
		
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir(tamanhoSenha);
		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeFalse();
		resultado.Errors.Should().ContainSingle().And
			.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES));
	}
	
	[Fact]
	public  void Validar_Erro_Telefone_Vazio()
	{
		var validator = new RegistrarUsuarioValidator();
		
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
		requisicao.Telefone = string.Empty;
		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeFalse();
		resultado.Errors.Should().ContainSingle().And
			.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.TELEFONE_USUARIO_EMBRANCO));
	}
	
	[Fact]
	public  void Validar_Erro_Telefone_Invalido()
	{
		var validator = new RegistrarUsuarioValidator();
		
		var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
		requisicao.Telefone = "21 9";
		var resultado = validator.Validate(requisicao);

		resultado.IsValid.Should().BeFalse();
		resultado.Errors.Should().ContainSingle().And
			.Contain(error => error.ErrorMessage.Equals(ResourceMensagensDeErro.TELEFONE_USUARIO_INVALIDO));
	}

}

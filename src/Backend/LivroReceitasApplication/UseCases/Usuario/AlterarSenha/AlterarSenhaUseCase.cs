using FluentValidation.Results;
using LivroReceitas.Application.Servicos.Criptografia;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Domain.Repositorio;
using LivroReceitas.Domain.Repositorio.Usuario;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;

namespace LivroReceitas.Application.UseCases.Usuario.AlterarSenha;
public class AlterarSenhaUseCase : IAlterarSenhaUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IUsuarioUpdateOnlyRepositorio _repositorio;
	private readonly EncriptadorDeSenha _encriptadorDeSenha;
	private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

	public AlterarSenhaUseCase(IUsuarioLogado usuarioLogado,IUsuarioUpdateOnlyRepositorio repositorio,
		EncriptadorDeSenha encriptadorDeSenha, IUnidadeDeTrabalho unidadeDeTrabalho)
	{
		_usuarioLogado = usuarioLogado;
		_repositorio = repositorio;
		_encriptadorDeSenha = encriptadorDeSenha;
		_unidadeDeTrabalho = unidadeDeTrabalho;
	}

	public async Task Executar(RequisicaoAlterarSenhaJson requisicao)
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

		var usuario = await _repositorio.RecuperarPorId(usuarioLogado.Id);
		Validar(requisicao, usuario);

		usuario.Senha = _encriptadorDeSenha.Criptografar(requisicao.NovaSenha);

		_repositorio.Update(usuario);
		await _unidadeDeTrabalho.Commit();
	}

	private void Validar(RequisicaoAlterarSenhaJson requisicao, Domain.Entidades.Usuario usuario)
	{
		var validator = new AlterarSenhaValidator();
		var resultado = validator.Validate(requisicao);
		var senhaAtualCripto = _encriptadorDeSenha.Criptografar(requisicao.SenhaAtual);

		if (!usuario.Senha.Equals(senhaAtualCripto))
		{
			resultado.Errors.Add(new ValidationFailure("senhaAtual", ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA));
		}

		if (!resultado.IsValid)
		{
			var mensagens = resultado.Errors.Select(x=> x.ErrorMessage).ToList();
			throw new ErrosDeValidacaoException(mensagens);
		}	
	}
}

using AutoMapper;
using LivroReceitas.Application.Servicos.Criptografia;
using LivroReceitas.Application.Servicos.Token;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Domain.Repositorio;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;


namespace LivroReceitas.Application.UseCases.Usuario.Registrar;

public class RegistrarUsuarioUseCase : IRegistrarUsuarioUseCase
{
	private readonly IUsuarioReadOnlyRepositorio _repositorioRead;
	private readonly IUsuarioWriteOnlyRepositorio _repositorio;
	private readonly IMapper _mapper;
	private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
	private readonly EncriptadorDeSenha _encriptadorDeSenha;
	private readonly TokenController _tokenController;

	public RegistrarUsuarioUseCase(IUsuarioReadOnlyRepositorio repositorioRead,
		IUsuarioWriteOnlyRepositorio repositorio, IMapper mapper, 
		IUnidadeDeTrabalho unidadeDeTrabalho, EncriptadorDeSenha encriptadorDeSenha, 
		TokenController tokenController)
	{
		_repositorioRead = repositorioRead;
		_repositorio = repositorio;
		_mapper = mapper;
		_unidadeDeTrabalho = unidadeDeTrabalho;
		_encriptadorDeSenha = encriptadorDeSenha;
		_tokenController = tokenController;
	}

	public async Task<RespostaUsuarioRegistradoJson> Executar(RequisicaoRegistrarUsuario requisicao)
	{
		await ValidarAsync(requisicao);

		var entidade = _mapper.Map<Domain.Entidades.Usuario>(requisicao);
		entidade.Senha = _encriptadorDeSenha.Criptografar(requisicao.Senha);
		await _repositorio.Adicionar(entidade);
		await _unidadeDeTrabalho.Commit();

		var token = _tokenController.GerarToken(entidade.Email);

		return new RespostaUsuarioRegistradoJson { Token= token };
	}

	private async Task ValidarAsync(RequisicaoRegistrarUsuario requisicao)
	{
		var validator = new RegistrarUsuarioValidator();
		var resultado = validator.Validate(requisicao);

		var existeUsuario = await _repositorioRead.ExiteUsuarioComEmail(requisicao.Email);
		if (existeUsuario)
		{
			resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMensagensDeErro.EMAIL_JA_REGISTRADO));
		}
		if (!resultado.IsValid)
		{
			var mensagensErro = resultado.Errors.Select(e => e.ErrorMessage).ToList();
			throw new ErrosDeValidacaoException(mensagensErro);
		}
	}
}

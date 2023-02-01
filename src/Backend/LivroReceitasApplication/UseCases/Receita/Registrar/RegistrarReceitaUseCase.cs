using AutoMapper;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Domain.Repositorio;
using LivroReceitas.Domain.Repositorio.Receita;
using LivroReceitas.Exceptions.ExceptionsBase;

namespace LivroReceitas.Application.UseCases.Receita.Registrar;
public class RegistrarReceitaUseCase : IRegistrarReceitaUseCase
{
	private readonly IMapper _mapper;
	private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IReceitaWriteOnlyRepositorio _receitaWriteOnlyRepositorio;

	public RegistrarReceitaUseCase(IMapper mapper, IUnidadeDeTrabalho unidadeDeTrabalho,
		IUsuarioLogado usuarioLogado, IReceitaWriteOnlyRepositorio receitaWriteOnlyRepositorio)
	{
		_mapper = mapper;
		_unidadeDeTrabalho = unidadeDeTrabalho;
		_usuarioLogado = usuarioLogado;
		_receitaWriteOnlyRepositorio = receitaWriteOnlyRepositorio;
	}

	public async Task<RespostaReceitaJson> Executar(RequisicaoReceitaJson requisicao)
	{
		Validar(requisicao);

		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
		var receita = _mapper.Map<Domain.Entidades.Receita>(requisicao);
		receita.UsuarioId = usuarioLogado.Id;

		await _receitaWriteOnlyRepositorio.Registrar(receita);

		await _unidadeDeTrabalho.Commit();

		return _mapper.Map<RespostaReceitaJson>(receita);
	}

	private void Validar(RequisicaoReceitaJson requisicao)
	{
		var validator = new RegistrarReceitaValidator();
		var resultado = validator.Validate(requisicao);
		if (!resultado.IsValid)
		{
			var mensagensDeErro = resultado.Errors.Select(c=> c.ErrorMessage).ToList();
			throw new ErrosDeValidacaoException(mensagensDeErro);
		}
	}
}  

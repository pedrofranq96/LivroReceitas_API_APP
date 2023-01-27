using AutoMapper;
using LivroReceitas.Application.Servicos.UsuarioLogado;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Domain.Extension;
using LivroReceitas.Domain.Repositorio.Receita;
using System.Globalization;

namespace LivroReceitas.Application.UseCases.DashBoard;
public class DashBoardUseCase : IDashBoardUseCase
{
	private readonly IReceitaReadOnlyRepositorio _repositorio;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IMapper _mapper;

	public DashBoardUseCase(IReceitaReadOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado, IMapper mapper)
	{
		_repositorio = repositorio;
		_usuarioLogado = usuarioLogado;
		_mapper = mapper;
	}

	public async Task<RespostaDashBoardJson> Executar(RequisicaoDashBoardJson requisicao)
	{
		var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
		var receitas = await _repositorio.RecuperarTodasDoUsuario(usuarioLogado.Id);

		receitas = Filtrar(requisicao, receitas);
		return new RespostaDashBoardJson
		{
			Receitas = _mapper.Map<List<RespostaReceitaDashBoardJson>>(receitas)
		};
	}

	private static IList<Domain.Entidades.Receita> Filtrar(RequisicaoDashBoardJson requisicao, IList<Domain.Entidades.Receita> receitas)
	{
		if(receitas is null)
		{
			return new List<Domain.Entidades.Receita>();
		}

		var receitasFiltradas = receitas;

		if (requisicao.Categoria.HasValue)
		{
			receitasFiltradas = receitas.Where(r => r.Categoria == (Domain.Enum.Categoria)requisicao.Categoria.Value).ToList();
		}
		if (!string.IsNullOrWhiteSpace(requisicao.TituloIngrediente))
		{
			
			receitasFiltradas = receitas.Where(r=> r.Titulo.CompararSemConsiderarAcentoUpperCase(requisicao.TituloIngrediente) 
			|| r.Ingredientes.Any(ingrediente => ingrediente.Produto.CompararSemConsiderarAcentoUpperCase(requisicao.TituloIngrediente))).ToList();
		}


		return receitasFiltradas.OrderBy(c=> c.Titulo).ToList();
	}
}

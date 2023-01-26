using AutoMapper;
using HashidsNet;
using LivroReceitas.Comunicacao.Requesicoes;

namespace LivroReceitas.Application.Servicos.AutoMapper;

public class AutoMapperConfig :Profile
{
	private readonly IHashids _hashids;
	public AutoMapperConfig(IHashids hashids)
	{
		_hashids = hashids;

		RequisicaoParaEntidade();
		EntidadeParaResposta();
	}

	private void RequisicaoParaEntidade()
	{
		CreateMap<RequisicaoRegistrarUsuarioJson, Domain.Entidades.Usuario>()
			.ForMember(destino => destino.Senha, config => config.Ignore());

		CreateMap<RequisicaoRegistrarReceitaJson, Domain.Entidades.Receita>();
		CreateMap<RequisicaoRegistrarIngredienteJson, Domain.Entidades.Ingrediente>();
			
	}
	private void EntidadeParaResposta()
	{
		CreateMap<Domain.Entidades.Receita, Comunicacao.Respostas.RespostaReceitaJson>()
			.ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));

		CreateMap<Domain.Entidades.Ingrediente, Comunicacao.Respostas.RespostaIngredienteJson>()
			.ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));
		
		CreateMap<Domain.Entidades.Receita, Comunicacao.Respostas.RespostaReceitaDashBoardJson>()
			.ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)))
			.ForMember(destino => destino.QuantidadeIngredientes, config => config.MapFrom(origem => origem.Ingredientes.Count));

	}
}

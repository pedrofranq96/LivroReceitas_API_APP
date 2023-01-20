using AutoMapper;
using LivroReceitas.Comunicacao.Requesicoes;

namespace LivroReceitas.Application.Servicos.AutoMapper;

public class AutoMapperConfig :Profile
{
	public AutoMapperConfig()
	{
		CreateMap<RequisicaoRegistrarUsuario, Domain.Entidades.Usuario>()
			.ForMember(destino => destino.Senha, config => config.Ignore());
	}
}

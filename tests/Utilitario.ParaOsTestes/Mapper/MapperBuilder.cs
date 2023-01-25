using AutoMapper;
using LivroReceitas.Application.Servicos.AutoMapper;
using UtilitarioParaOsTestes.HashIds;

namespace UtilitarioParaOsTestes.Mapper;

public class MapperBuilder
{
	public static IMapper Instancia()
	{
		var hashids = HashidsBuilder.Instance().Build();

		var mockMapper = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new AutoMapperConfig(hashids));
		});
		return mockMapper.CreateMapper();
	}
}

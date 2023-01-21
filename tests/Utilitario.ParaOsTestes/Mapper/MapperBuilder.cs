using AutoMapper;
using LivroReceitas.Application.Servicos.AutoMapper;

namespace UtilitarioParaOsTestes.Mapper;

public class MapperBuilder
{
	public static IMapper Instancia()
	{
		var config = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<AutoMapperConfig>();
		});
		return config.CreateMapper();
	}
}

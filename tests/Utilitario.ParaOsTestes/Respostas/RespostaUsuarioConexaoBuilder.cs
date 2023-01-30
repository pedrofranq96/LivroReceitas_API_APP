using Bogus;
using LivroReceitas.Comunicacao.Respostas;
using UtilitarioParaOsTestes.HashIds;

namespace UtilitarioParaOsTestes.Respostas;
public class RespostaUsuarioConexaoBuilder
{
	public static RespostaUsuarioConexaoJson Construir()
	{
		var hashids = HashidsBuilder.Instance().Build();

		return new Faker<RespostaUsuarioConexaoJson>()
			.RuleFor(c => c.Id, f => hashids.EncodeLong(f.Random.Long(1,5000)))
			.RuleFor(c => c.Nome, f => f.Person.FullName);
	}
}

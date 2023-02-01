using System.Collections;
using UtilitarioParaOsTestes.Entidades;

namespace UseCase.Test.Conexao.InlineData;
public class EntidadesUsuarioConexaoDataTeste : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		var conexoes = ConexaoBuilder.Construir();

		return conexoes.Select(conexao => new object[] { conexao.Id, conexoes }).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

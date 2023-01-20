using Dapper;
using MySqlConnector;

namespace LivroReceitas.Infra.Migrations;

public static class Database
{
	public static void CriarDB(string conexao, string nomeDB)
	{
		using var minhaConexao = new MySqlConnection(conexao);

		var parametros = new DynamicParameters();
		parametros.Add("nome", nomeDB);
		var registros = minhaConexao.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @nome", parametros);
		if (!registros.Any())
		{
			minhaConexao.Execute($"CREATE DATABASE {nomeDB}");
		}
	}
}

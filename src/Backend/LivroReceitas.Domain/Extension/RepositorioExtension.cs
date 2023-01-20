using Microsoft.Extensions.Configuration;

namespace LivroReceitas.Domain.Extension;

public static class RepositorioExtension
{
	public static string GetNomeDB(this IConfiguration configurationManager) 
	{
		var nomeDb = configurationManager.GetConnectionString("Database");
		return nomeDb;
	}
	
	public static string GetConnection(this IConfiguration configurationManager) 
	{
		var conexao = configurationManager.GetConnectionString("Conexao");
		return conexao;
	}

	public static string GetConexaoCompleta(this IConfiguration configurationManager)
	{
		var nomeDb = configurationManager.GetNomeDB();
		var conexao = configurationManager.GetConnection();

		return $"{conexao}Database={nomeDb}";
	}
}

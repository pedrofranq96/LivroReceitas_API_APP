using FluentMigrator;

namespace LivroReceitas.Infra.Migrations.Versoes;

[Migration((long)NumeroVersoes.AlterarTabelaReceitas, "Adicionando coluna tempo de preparo")]
public class Versao0000003 : Migration
{
	public override void Down()
	{
		
	}

	public override void Up()
	{
		Alter.Table("receitas").AddColumn("TempoPreparo").AsInt32().NotNullable().WithDefaultValue(0);
	}
}

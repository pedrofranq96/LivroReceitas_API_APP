﻿using FluentMigrator;
using System.Diagnostics.Metrics;

namespace LivroReceitas.Infra.Migrations.Versoes;
[Migration((long)NumeroVersoes.CriarTabelaAssociacaoUsuario, "Adicionando tabelas para associacao de usuarios")]

public class Versao0000004 : Migration
{
	public override void Down()
	{

	}

	public override void Up()
	{
		var tabela = VersaoBase.InserirColunasPadrao(Create.Table("Codigos"));

		tabela
			.WithColumn("Codigo").AsString(2000).NotNullable()

			.WithColumn("UsuarioId").AsInt64().NotNullable().ForeignKey("FK_Codigo_Usuario_Id", "Usuarios", "Id");
	}
}

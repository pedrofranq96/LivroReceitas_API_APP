﻿using LivroReceitas.Application.Servicos.Criptografia;

namespace UtilitarioParaOsTestes.Criptografia;

public class EncriptadorDeSenhaBuilder
{
	public static EncriptadorDeSenha Instancia()
	{
		return new EncriptadorDeSenha("ABCD123");
	}
}

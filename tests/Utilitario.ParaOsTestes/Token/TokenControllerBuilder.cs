using LivroReceitas.Application.Servicos.Criptografia;
using LivroReceitas.Application.Servicos.Token;

namespace UtilitarioParaOsTestes.Token;

public class TokenControllerBuilder
{
	public static TokenController Instancia()
	{
		return new TokenController(1000, "eVpwSjRENzRuJGQ5YypDcjBSNWgzSzExM15ub2gwTyp0OTk3YThRU1RpSUpeeVRyKlY=");
	}
}

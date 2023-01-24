using LivroReceitas.Application.Servicos.Criptografia;
using LivroReceitas.Application.Servicos.Token;

namespace UtilitarioParaOsTestes.Token;

public class TokenControllerBuilder
{
	public static TokenController Instancia()
	{
		return new TokenController(1000, "NVkkYVRMQCRwdjVEU01qOGJGIzlCdk0kaXpwNTI4");
	}
	
	public static TokenController TokenExpirado()
	{
		return new TokenController(0.0166667, "NVkkYVRMQCRwdjVEU01qOGJGIzlCdk0kaXpwNTI4");
	}
}

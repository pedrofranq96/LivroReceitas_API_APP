using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LivroReceitas.Application.Servicos.Token;

public class TokenController
{
	private const string EmailAlias = "eml";
	private readonly double _tempoDeVidaTokenEmMinutos;
	private readonly string _chaveDeSeguranca;

	public TokenController(double tempoDeVidaTokenEmMinutos, string chaveDeSeguranca)
	{
		_tempoDeVidaTokenEmMinutos = tempoDeVidaTokenEmMinutos;
		_chaveDeSeguranca = chaveDeSeguranca;
	}

	public string GerarToken(string emailUsuario)
	{
		var claims = new List<Claim>
		{
			new Claim(EmailAlias, emailUsuario),
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(_tempoDeVidaTokenEmMinutos),
			SigningCredentials = new SigningCredentials(SimetricKey(), SecurityAlgorithms.HmacSha256Signature)
		};
		var securityToken = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(securityToken);
	}

	public ClaimsPrincipal ValidarToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		var paramentrosValidacao = new TokenValidationParameters 
		{
			RequireExpirationTime = true,
			IssuerSigningKey = SimetricKey(),
			ClockSkew = new TimeSpan(0),
			ValidateIssuer = false,
			ValidateAudience = false,
		};
		
		var claims = tokenHandler.ValidateToken(token, paramentrosValidacao, out _);
		return claims;
	}

	public string RecuperarEmail(string token)
	{
		var claim = ValidarToken(token);

		return claim.FindFirst(EmailAlias).Value;

	}

	private SymmetricSecurityKey SimetricKey()
	{
		var symmetricKey = Convert.FromBase64String(_chaveDeSeguranca);
		return new SymmetricSecurityKey(symmetricKey);
	}
}

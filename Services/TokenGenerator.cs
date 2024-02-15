using DemoApi.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoApi.Services
{
	public class TokenGenerator
	{
		private readonly IConfiguration _configuration;
		public TokenGenerator(IConfiguration configuration)
		{
			_configuration = configuration;
		}



		public async Task<string> GenerateToken(IdentityUser user)
		{
			if (user == null)
			{
				return string.Empty;
			}

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:secretKey"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var authClaims = new List<Claim>
			{
					new Claim(ClaimTypes.NameIdentifier, user.Id),
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
							};
							
			var token = new JwtSecurityToken(
                    issuer: _configuration["jwt:validIssuer"],
                    audience: _configuration["jwt:validAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: authClaims,
                    signingCredentials: credentials
                    );
				
			return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
		}
	}
}
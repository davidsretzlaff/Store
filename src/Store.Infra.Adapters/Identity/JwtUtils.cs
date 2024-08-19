using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Domain.Interface.Infra.Adapters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store.Infra.Adapters.Identity
{
    public class JwtUtils : IJwtUtils
	{
		private readonly IConfiguration _configuration;

		public JwtUtils(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(string userName, string role, string companyRegisterNumber)
		{
			var jwtSettings = _configuration["JwtSettings:SecretKey"];

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(jwtSettings);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, userName.ToString()),
					new Claim("CompanyRegisterNumber", companyRegisterNumber),
					new Claim(ClaimTypes.Role, role) 
				}),
				Expires = DateTime.UtcNow.AddMinutes(20),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature
				)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public List<string> ValidateToken(string token)
		{

			if (token == null)
				return new List<string>();

			var tokenHandler = new JwtSecurityTokenHandler();

			var jwtSettings = _configuration["JwtSettings:SecretKey"];

			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings)),
				ValidateIssuer = false,
				ValidateAudience = false,

				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;
			if (jwtToken != null)
			{
				var roles = new List<string>();
				foreach (var claim in jwtToken.Claims)
				{
					if (claim.Type.ToLower() == "role")
					{
						roles.Add(claim.Value);
					}
				}
				return roles;
			}

			return new List<string>();
		}
	}
}


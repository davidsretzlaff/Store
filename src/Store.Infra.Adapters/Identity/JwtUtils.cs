using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Application.Common.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store.Infra.Adapters.Identity
{
    public class JwtUtils : IJwtUtils
	{
		//private readonly string _key;
		//private readonly string _issuer;
		//private readonly string _audience;
		//private readonly string _expiryMinutes;
		private readonly IConfiguration _configuration;

		public JwtUtils(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(string userName, string role)
		{
			var jwtSettings = _configuration["JwtSettings:SecretKey"];

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(jwtSettings);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, userName.ToString()),
					new Claim(ClaimTypes.Role, role) 
				}),
				Expires = DateTime.UtcNow.AddHours(8),
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
			//Guard.Against.Null(jwtSettings, message: "JwtOptions not found.");
			//var key = Guard.Against.NullOrEmpty(jwtSettings["Secret"], message: "'Secret' not found or empty.");

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

			// return user roles from JWT token if validation successful
			return new List<string>();
		}
	}
}


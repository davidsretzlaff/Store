using Microsoft.IdentityModel.Tokens;
using Store.Application.Exceptions;
using Store.Application.Interface;
using Store.Application.UseCases.User.Auth.CreateAuth;
using Store.Domain.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store.Application.UseCases.Auth.CreateAuth
{
	public class CreateAuth : ICreateAuth
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly string _jwtSecretKey;

		public CreateAuth(IUserRepository userRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
			//_jwtSecretKey = jwtSecretKey;
		}
		public async Task<AuthOutput> Handle(CreateAuthInput input, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByUserName(input.UserName, cancellationToken);
			if (user.Password != input.Password) 
			{
				PasswordInvalidException.ThrowIfPasswordInvalid();
			}

			var token = GenerateToken(user);
			return AuthOutput.FromUser(user, token);
		}
		private string GenerateToken(Domain.Entity.User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSecretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.UserName.ToString())
					//new Claim(ClaimTypes.Role, user.Role.ToString()) fazer se der tempo
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
	}
}

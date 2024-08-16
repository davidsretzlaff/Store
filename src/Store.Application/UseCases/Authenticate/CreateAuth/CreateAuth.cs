using Microsoft.IdentityModel.Tokens;
using Store.Application.Exceptions;
using Store.Application.Interface;
using Store.Application.UseCases.Authenticate.CreateAuthenticate;
using Store.Application.UseCases.User.Common;
using Store.Domain.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.User.CreateAuthenticate
{
	public class CreateAuth : ICreateAuth
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CreateAuth(IUserRepository userRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
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
			var key = Encoding.ASCII.GetBytes("4a8b21d3c5e789abf1e2cd9f34a7b2c1e89d4f5a6c3d7b8f9e1a2c3b4d5e6f7a");
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

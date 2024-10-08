﻿using Microsoft.IdentityModel.Tokens;
using Store.Application.Common.Exceptions;
using Store.Application.UseCases.User.Auth.CreateAuth;
using Store.Domain.Interface.Infra.Adapters;
using Store.Domain.Interface.Infra.Repository;
using Store.Domain.SeedWork;

namespace Store.Application.UseCases.Auth.CreateAuth
{
    public class CreateAuth : ICreateAuth
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IJwtUtils _jwtUtils;
		private readonly string _jwtSecretKey;

		public CreateAuth(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtUtils jwtUtils)
		{
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
			_jwtUtils = jwtUtils;
			_jwtSecretKey = string.Empty;
		}
		public async Task<AuthOutput> Handle(CreateAuthInput input, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByUserName(input.UserName, cancellationToken);
			RelatedAggregateException.ThrowIfNull(user, $"User '{input.UserName}' not found.");
			
			if (user!.Password != input.Password) 
			{
				PasswordInvalidException.ThrowIfPasswordInvalid();
			}

			var token = _jwtUtils.GenerateToken(user.UserName, Roles.BasicUser, user.CompanyIdentificationNumber.Value);
			return AuthOutput.FromUser(user, token);
		}
	}
}

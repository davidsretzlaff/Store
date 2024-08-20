using Store.Application.Common.Exceptions;
using Store.Application.UseCases.User.Common;
using Store.Domain.Interface.Infra.Repository;
using Store.Domain.Validation;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.User.CreateUser
{
    public class CreateUser : ICreateUser
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CreateUser(IUserRepository userRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
		}
		public async Task<UserOutput> Handle(CreateUserInput input, CancellationToken cancellationToken)
		{
			var user = new DomainEntity.User(
				input.UserName,
				input.Password,
				input.BusinessName,
				input.CorporateName,
				input.Email,
				input.SiteUrl,
				input.Phone,
				input.Cnpj,
				input.Address.ToDomainAddress()
			);

			await VerifyUserExistence(input, cancellationToken);
			await _userRepository.Insert(user, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return UserOutput.FromUser(user);
		}

		private async Task VerifyUserExistence(CreateUserInput input , CancellationToken cancellationToken)
		{
			var existingUser = await _userRepository.GetByUserNameOrcompanyIdentificationNumber(input.UserName, input.Cnpj, cancellationToken);
			
			if (existingUser is null) 
			{
				return;
			}

			if (existingUser.IsUserNameMatching(input.UserName))
			{
				UserNameExistsException.ThrowIfNull($"'{input.UserName}' already exists.");
			}
			if (existingUser.CompanyIdentificationNumber.isMatch(input.Cnpj))
			{
				CnpjExistsException.ThrowIfNull($"'{input.Cnpj}' already exists.");
			}
		}
	}
}

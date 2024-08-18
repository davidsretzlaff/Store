using Store.Application.Common.Exceptions;
using Store.Application.Common.Interface;
using Store.Application.UseCases.User.Common;
using Store.Domain.Repository;
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
				input.CompanyRegistrationNumber,
				input.Address.ToDomainAddress()
			);

			await VerifyUserExistence(input, cancellationToken);
			await _userRepository.Insert(user, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return UserOutput.FromUser(user);
		}

		private async Task VerifyUserExistence(CreateUserInput input , CancellationToken cancellationToken)
		{
			var existingUser = await _userRepository.GetByUserNameOrCompanyRegNumber(input.UserName, input.CompanyRegistrationNumber, cancellationToken);
			
			if (existingUser is null) 
			{
				return;
			}

			ValidateUserName(existingUser.UserName, input.UserName);
			ValidateCompanyRegistrationNumber(existingUser.CompanyRegistrationNumber, input.CompanyRegistrationNumber);
		}
		private void ValidateUserName(string existingUserName, string newUserName)
		{
			if (existingUserName.Equals(newUserName))
			{
				UserNameExistsException.ThrowIfNull($"'{newUserName}' already exists.");
			}
		}
		private void ValidateCompanyRegistrationNumber(string existingCompanyRegNumber, string newCompanyRegNumber)
		{
			if (existingCompanyRegNumber.Equals(newCompanyRegNumber))
			{
				CompanyRegistrationNumberExistsException.ThrowIfNull($"'{newCompanyRegNumber}' already exists.");
			}
		}
	}
}

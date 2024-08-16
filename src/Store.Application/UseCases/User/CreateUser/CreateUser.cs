using Store.Application.Interface;
using Store.Application.UseCases.User.Common;
using Store.Domain.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.User.CreateUser
{
    public class CreateAuthenticate : ICreateUser
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CreateAuthenticate(IUserRepository userRepository, IUnitOfWork unitOfWork
		)
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
			await _userRepository.Insert(user, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return UserOutput.FromUser(user);
		}
	}
}

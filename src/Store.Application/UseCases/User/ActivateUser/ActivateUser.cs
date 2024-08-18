using Store.Application.UseCases.User.CreateUser;
using DomainEntity = Store.Domain.Entity;
using Store.Application.UseCases.User.Common;
using Store.Domain.Interface.Repository;

namespace Store.Application.UseCases.User.ActivateUser
{
    public class ActivateUser : IActivateUser
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ActivateUser(IUserRepository userRepository, IUnitOfWork unitOfWork
		)
		{
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
		}
		public async Task<UserOutput> Handle(ActivateUserInput input, CancellationToken cancellationToken)
		{
			var user = await _userRepository.Get(input.id, cancellationToken);
			user.Activate();
			await _userRepository.Update(user, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return UserOutput.FromUser(user);
		}
	}
}

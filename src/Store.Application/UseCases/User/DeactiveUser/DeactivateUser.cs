using Store.Application.UseCases.User.ActivateUser;
using Store.Application.UseCases.User.Common;
using Store.Domain.Interface.Infra.Repository;

namespace Store.Application.UseCases.User.DeactiveUser
{
    internal class DeactivateUser : IDeactivateUser
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;

		public DeactivateUser(IUserRepository userRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
		}
		public async Task<UserOutput> Handle(DeactivateUserInput input, CancellationToken cancellationToken)
		{
			var user = await _userRepository.Get(input.id, cancellationToken);
			user.Deactivate();
			await _userRepository.Update(user, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return UserOutput.FromUser(user);
		}
	}
}

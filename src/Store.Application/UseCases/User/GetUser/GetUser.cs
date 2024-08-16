using Store.Application.Interface;
using Store.Application.UseCases.User.Common;
using Store.Domain.Repository;

namespace Store.Application.UseCases.User.GetUser
{
	public class GetUser : IGetUser
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;

		public GetUser(IUserRepository userRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
		}

		public async Task<UserOutput> Handle(GetUserInput input, CancellationToken cancellationToken)
		{
			var user = await _userRepository.Get(input.Id, cancellationToken);
			return UserOutput.FromUser(user);
		}
	}
}

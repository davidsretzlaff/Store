using Store.Application.Common.Exceptions;
using Store.Domain.Interface.Repository;
using Store.Domain.Interface.Application;

namespace Store.Application.Common.UserValidation
{
	public class UserValidation : IUserValidation
	{

        private readonly IUserRepository _user;

        public UserValidation(IUserRepository user)
        {
            _user = user;
        }

		public async Task IsUserActive(string companyRegisterNumber, CancellationToken cancellationToken)
        {
            var user = await _user.GetByUserNameOrCompanyRegNumber(null, companyRegisterNumber, cancellationToken);

            if (user is null || !user.IsActive())
            {
                throw new UserInactiveException($"User with CompanyRegisterNumber '{companyRegisterNumber}' is not active. Only Active users can create an order.");
            }
        }
	}
}

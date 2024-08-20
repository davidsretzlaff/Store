using Store.Application.Common.Exceptions;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;

namespace Store.Application.Common.UserValidation
{
    public class UserValidation : IUserValidation
	{

        private readonly IUserRepository _user;

        public UserValidation(IUserRepository user)
        {
            _user = user;
        }

		public async Task IsUserActive(string Cnpj, CancellationToken cancellationToken)
        {
            var user = await _user.GetByUserNameOrCnpj(null, Cnpj, cancellationToken);

            if (user is null || !user.IsActive())
            {
                throw new UserInactiveException($"User with Cnpj '{Cnpj}' is not active. Only Active users can create an order.");
            }
        }
	}
}

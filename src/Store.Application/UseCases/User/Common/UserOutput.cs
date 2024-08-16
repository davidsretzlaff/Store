using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.User.Common
{
    public record UserOutput
    (
        Guid Id,
        string UserName,
        string BusinessName,
        string CorporateName,
        string Email,
        string SiteUrl,
        string Phone,
        string CompanyRegistrationNumber,
        string Status,
        AddressOutput Address
    )
    {
        public static UserOutput FromUser(DomainEntity.User user)
        {
            return new UserOutput(
				   user.Id,
                   user.UserName,
                   user.BusinessName,
                   user.CorporateName,
                   user.Email,
                   user.SiteUrl,
                   user.Phone,
                   user.CompanyRegistrationNumber,
                   user.Status.ToStringStatus(),
                   new AddressOutput(user.Address.Street, user.Address.City, user.Address.State, user.Address.Country, user.Address.ZipCode)
               );
        }
    }
}

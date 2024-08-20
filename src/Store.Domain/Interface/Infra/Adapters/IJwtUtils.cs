namespace Store.Domain.Interface.Infra.Adapters
{
    public interface IJwtUtils
    {
        string GenerateToken(string username, string role, string companyIdentificationNumber);
        List<string> ValidateToken(string token);
    }
}
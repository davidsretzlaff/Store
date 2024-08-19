namespace Store.Application.Common.Interface
{
    public interface IJwtUtils
    {
        string GenerateToken(string username, string role, string companyRegisterNumber);
        List<string> ValidateToken(string token);
	}
}
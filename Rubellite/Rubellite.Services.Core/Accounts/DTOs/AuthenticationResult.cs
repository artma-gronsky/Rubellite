namespace Rubellite.Services.Core.Accounts.DTOs;

public class AuthenticationResult
{
    public string Token { get; set; }
    
    public DateTime Expiration { get; set; }
}
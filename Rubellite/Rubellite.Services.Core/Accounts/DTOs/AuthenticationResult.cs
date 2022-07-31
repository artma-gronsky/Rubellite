namespace Rubellite.Services.Core.Accounts.DTOs;

public class AuthenticationResult
{
    public string Token { get; set; }
    
    public DateTime TokenExpiryTime { get; set; }
    
    public string RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }
}
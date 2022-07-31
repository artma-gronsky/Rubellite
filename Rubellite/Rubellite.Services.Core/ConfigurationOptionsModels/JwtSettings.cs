namespace Rubellite.Services.Core.ConfigurationOptionsModels;

public class JwtSettings
{
    public string Key { get; set; }
    
    public int TokenExpirationInMinutes { get; set; }
    
    public int RefreshTokenExpirationsInDays { get; set; }
}
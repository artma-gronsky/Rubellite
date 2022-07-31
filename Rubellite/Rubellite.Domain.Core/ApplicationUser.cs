using Microsoft.AspNetCore.Identity;

namespace Rubellite.Domain.Core;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
using System.Security.Claims;

namespace Rubellite.Services.Core.Authorization;

public static class RubelliteRolesConfigurations
{
    private static readonly Dictionary<RubelliteRoles, Claim> RubelliteRolesClaim = new()
    {
        { RubelliteRoles.Administrator, new Claim("role", "admin") }
    };

    public static Claim GetRequiredClaim(this RubelliteRoles roles)
    {
        return RubelliteRolesClaim[roles];
    }
    
    public static string GetPolicyName(this RubelliteRoles roles)
    {
        return roles.ToString();
    }
}
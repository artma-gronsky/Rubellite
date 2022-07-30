using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Rubellite.Services.Core.Authorization;

namespace Rubellite.WebApp.Core.Attributes;

public class AdminAuthorizationAttribute: AuthorizeAttribute
{
    public AdminAuthorizationAttribute()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        Policy = RubelliteRoles.Administrator.GetPolicyName();
    }
}
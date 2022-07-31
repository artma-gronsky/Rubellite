using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Rubellite.WebApp.Core.Attributes;

public class AuthorizeAttribute: Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    public AuthorizeAttribute()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}
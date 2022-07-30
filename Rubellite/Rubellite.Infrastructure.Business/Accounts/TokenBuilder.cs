using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rubellite.Services.Core.Accounts.DTOs;
using Rubellite.Services.Core.Authorization;
using Rubellite.Services.Core.ConfigurationOptionsModels;

namespace Rubellite.Infrastructure.Business.Accounts;

public class TokenBuilder : ITokenBuilder
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwfSettings;

    public TokenBuilder(UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwfSettings)
    {
        _userManager = userManager;
        _jwfSettings = jwfSettings.Value;
    }

    public async Task<AuthenticationResult> BuildToken(UserCredentials userCredentials)
    {
        var claims = new List<Claim> { new (CustomClaimTypes.PreferredUsername, userCredentials.Email) };

        var user = await _userManager.FindByNameAsync(userCredentials.Email);
        var claimsDb = await _userManager.GetClaimsAsync(user);

        claims.AddRange(claimsDb);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwfSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddYears(1);

        var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
            expires: expiration, signingCredentials: credentials);

        return new AuthenticationResult()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
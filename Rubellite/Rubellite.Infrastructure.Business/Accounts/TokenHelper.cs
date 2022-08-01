using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rubellite.Domain.Core;
using Rubellite.Services.Core.Accounts.DTOs;
using Rubellite.Services.Core.Authorization;
using Rubellite.Services.Core.ConfigurationOptionsModels;
using Rubellite.Services.Core.Exceptions;
using Rubellite.Services.Interfaces.Accounts;

namespace Rubellite.Infrastructure.Business.Accounts;

public class TokenHelper : ITokenHelper
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwfSettings;

    public TokenHelper(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwfSettings)
    {
        _userManager = userManager;
        _jwfSettings = jwfSettings.Value;
    }

    public async Task<AuthenticationResult> CreateToken(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var claims = await _userManager.GetClaimsAsync(user);
        
        claims.Add(new Claim(RubelliteCustomClaims.UserId, id));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwfSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(_jwfSettings.TokenExpirationInMinutes);

        var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
            expires: expiration, signingCredentials: credentials);

        var refreshToken = CreateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwfSettings.RefreshTokenExpirationsInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

        await _userManager.UpdateAsync(user);
        
        return new AuthenticationResult()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            TokenExpiryTime = expiration,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime
        };
    }


    public async Task<AuthenticationResult> ObtainNewTokenByRefreshToken(TokenModel tokenModel)
    {
        if (tokenModel is null)
        {
            throw new InputModelException("Invalid client request");
        }

        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            throw new InputModelException("Invalid access token or refresh token");
        }

        var id = principal.Claims.First(x => x.Type == RubelliteCustomClaims.UserId).Value;
        
        var user = await _userManager.FindByIdAsync(id);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new InputModelException("Invalid access token or refresh token");
        }

        return await CreateToken(user.Id);
    }
    
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwfSettings.Key)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
    
    private string CreateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
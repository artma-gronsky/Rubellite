using Rubellite.Services.Core.Accounts.DTOs;

namespace Rubellite.Services.Interfaces.Accounts;

public interface ITokenHelper
{
    Task<AuthenticationResult> CreateToken(string id);

    Task<AuthenticationResult> ObtainNewTokenByRefreshToken(TokenModel tokenModel);
}
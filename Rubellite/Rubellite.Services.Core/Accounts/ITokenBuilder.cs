using Rubellite.Services.Core.Accounts.DTOs;

namespace Rubellite.Infrastructure.Business.Accounts;

public interface ITokenBuilder
{
    Task<AuthenticationResult> BuildToken(UserCredentials userCredentials);
}
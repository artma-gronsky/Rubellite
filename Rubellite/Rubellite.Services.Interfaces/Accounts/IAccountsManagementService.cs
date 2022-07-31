using Rubellite.Services.Core.Accounts.DTOs;
using Rubellite.Services.Core.Authorization;

namespace Rubellite.Services.Interfaces.Accounts;

public interface IAccountsManagementService
{
    Task Register(UserCredentials userCredentials);
    Task<AuthenticationResult> Login(UserCredentials userCredentials);
    Task<AuthenticationResult> RefreshAccess(string userId, TokenModel tokenModel);
    Task GrantNewRole(RubelliteRoles roles, string userId);
    Task TakeOffRole(RubelliteRoles roles, string userId);
}
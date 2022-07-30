using Rubellite.Services.Core.Accounts.DTOs;
using Rubellite.Services.Core.Authorization;

namespace Rubellite.Infrastructure.Business.Accounts;

public interface IAccountsManagementService
{
    Task<AuthenticationResult> Register(UserCredentials userCredentials);
    Task<AuthenticationResult> Login(UserCredentials userCredentials);
    Task GrantNewRole(RubelliteRoles roles, string userId);
    Task TakeOffRole(RubelliteRoles roles, string userId);
}
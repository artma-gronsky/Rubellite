using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Rubellite.Domain.Core;
using Rubellite.Services.Core.Accounts;
using Rubellite.Services.Core.Accounts.DTOs;
using Rubellite.Services.Core.Authorization;
using Rubellite.Services.Interfaces.Accounts;

namespace Rubellite.Infrastructure.Business.Accounts;

public class AccountsManagementService : IAccountsManagementService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    
    private readonly ITokenHelper _tokenHelper;

    public AccountsManagementService(UserManager<ApplicationUser> userManager, ITokenHelper tokenHelper, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _tokenHelper = tokenHelper;
        _signInManager = signInManager;
    }

    public async Task Register(UserCredentials userCredentials)
    {
        var user = new ApplicationUser { UserName = userCredentials.Email, Email = userCredentials.Email};
        var result = await _userManager.CreateAsync(user, userCredentials.Password);

        if (!result.Succeeded)
            throw new BadHttpRequestException(string.Join("\n ", result.Errors.Select(x => $"{x.Code} {x.Description}")));
    }
    
    public async Task<AuthenticationResult> Login(UserCredentials userCredentials)
    {
        var result = await _signInManager.PasswordSignInAsync(userCredentials.Email,
            userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
        
        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(userCredentials.Email);
            
            return await _tokenHelper.CreateToken(user.Id);
        }
        
        throw new BadHttpRequestException("Incorrect login or password");
    }

    public async Task<AuthenticationResult> RefreshAccess(string userId, TokenModel tokenModel)
    {
         await _tokenHelper.ThrowIfCanNotRefreshToken(tokenModel);
         
         return await _tokenHelper.CreateToken(userId);
    }
    
    public async Task GrantNewRole(RubelliteRoles roles, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        await _userManager.AddClaimAsync(user, roles.GetRequiredClaim());
    }

    public async Task TakeOffRole(RubelliteRoles roles, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        await _userManager.RemoveClaimAsync(user, roles.GetRequiredClaim());
    }
    
    
}
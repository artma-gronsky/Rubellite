using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Rubellite.Services.Core.Accounts.DTOs;
using Rubellite.Services.Core.Authorization;

namespace Rubellite.Infrastructure.Business.Accounts;

public class AccountsManagementService : IAccountsManagementService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    
    private readonly ITokenBuilder _tokenBuilder;

    public AccountsManagementService(UserManager<IdentityUser> userManager, ITokenBuilder tokenBuilder, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _tokenBuilder = tokenBuilder;
        _signInManager = signInManager;
    }

    public async Task<AuthenticationResult> Register(UserCredentials userCredentials)
    {
        var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email};
        var result = await _userManager.CreateAsync(user, userCredentials.Password);

        if (result.Succeeded)
        {
            return await _tokenBuilder.BuildToken(userCredentials);
        }

        throw new BadHttpRequestException(string.Join("\n ", result.Errors.Select(x => $"{x.Code} {x.Description}")));
    }
    
    public async Task<AuthenticationResult> Login(UserCredentials userCredentials)
    {
        var result = await _signInManager.PasswordSignInAsync(userCredentials.Email,
            userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return await _tokenBuilder.BuildToken(userCredentials);
        }
        
        throw new BadHttpRequestException("Incorrect Login");
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
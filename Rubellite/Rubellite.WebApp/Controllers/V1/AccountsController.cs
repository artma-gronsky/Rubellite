using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rubellite.Infrastructure.Business.Accounts;
using Rubellite.Services.Core.Accounts.DTOs;
using Rubellite.Services.Core.Authorization;

namespace Rubellite.WebApp.Controllers.V1;

[ApiController]
[Route("v1/api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountsManagementService _accountsManagementService;

    public AccountsController(IAccountsManagementService accountsManagementService)
    {
        _accountsManagementService = accountsManagementService;
    }
    
    [HttpPut("SignUp")]
    public async Task<ActionResult<AuthenticationResult>> SignUp(
        [FromBody] UserCredentials userCredentials)
    {
        return Ok(await _accountsManagementService.Register(userCredentials));
    }

    [HttpPost("SignIn")]
    public async Task<ActionResult<AuthenticationResult>> SignIn(
        [FromBody] UserCredentials userCredentials)
    {
        return Ok(await _accountsManagementService.Login(userCredentials));
    }

    [Authorize]
    [HttpPost("Info")]
    public async Task<string?> Info()
    {
        return User.FindFirst(CustomClaimTypes.PreferredUsername)?.Value;
    }
}
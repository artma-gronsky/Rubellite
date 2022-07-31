using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Rubellite.Services.Core.Accounts.DTOs;
using Rubellite.Services.Core.Authorization;
using Rubellite.Services.Interfaces.Accounts;
using Rubellite.WebApp.Core.Attributes;

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
    public async Task<ActionResult> SignUp(
        [FromBody] UserCredentials userCredentials)
    {
        await _accountsManagementService.Register(userCredentials);
        return Ok();
    }

    [HttpPost("SignIn")]
    public async Task<ActionResult<AuthenticationResult>> SignIn(
        [FromBody] UserCredentials userCredentials)
    {
        return Ok(await _accountsManagementService.Login(userCredentials));
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<AuthenticationResult>> RefreshToken(
        [FromBody] TokenModel tokenModel)
    {
        return Ok(await _accountsManagementService.RefreshAccess(tokenModel));
    }
    
    [Authorize]
    [HttpGet("info")]
    public ActionResult Info()
    {
        return Ok(User.FindFirstValue(RubelliteCustomClaims.UserId));
    }
}
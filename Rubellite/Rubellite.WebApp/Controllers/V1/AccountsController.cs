using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rubellite.Services.Core.Accounts.DTOs;
using Rubellite.Services.Core.Authorization;
using Rubellite.Services.Interfaces.Accounts;

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
    
    [Authorize]
    [HttpPost("RefreshToken")]
    public async Task<ActionResult<AuthenticationResult>> RefreshToken(
        [FromBody] TokenModel tokenModel)
    {
        var id = User.FindFirstValue(RubelliteCustomClaims.UserId);
        return Ok(await _accountsManagementService.RefreshAccess(id, tokenModel));
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("info")]
    public ActionResult Info()
    {
        return Ok(User.FindFirstValue(RubelliteCustomClaims.UserId));
    }
}
using System.ComponentModel.DataAnnotations;

namespace Rubellite.Services.Core.Accounts.DTOs;

public class UserCredentials
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}
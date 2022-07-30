using System.ComponentModel.DataAnnotations;

namespace Rubellite.Services.Core.ConfigurationOptionsModels;

public class DatabaseSettings
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Host { get; set; } = null!;

    [Required]
    public int Port { get; set; }
        
    [Required]
    public string Database { get; set; } = null!;
}
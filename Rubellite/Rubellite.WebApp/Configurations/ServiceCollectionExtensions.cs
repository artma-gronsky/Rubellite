using System.Text;
using Rubellite.Services.Core.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Rubellite.Domain.Core;
using Rubellite.Domain.Interfaces;
using Rubellite.Infrastructure.Business;
using Rubellite.Infrastructure.Business.Accounts;
using Rubellite.Infrastructure.Data.DbContext;
using Rubellite.Infrastructure.Data.Repositories;
using Rubellite.Services.Core.Accounts;
using Rubellite.Services.Core.ConfigurationOptionsModels;
using Rubellite.Services.Interfaces;
using Rubellite.Services.Interfaces.Accounts;

namespace Rubellite.WebApp.Configurations;

public static class ServiceCollectionExtensions
{
    public static void RegisterConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
    }
    
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITicketRepository, TicketRepository>();
    }
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenHelper, TokenHelper>();
        
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IAccountsManagementService, AccountsManagementService>();
    }
    
    public static void SetIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<RubelliteContext>()
            .AddDefaultTokenProviders();
    }

    public static void SetAuthenticationConfiguration(this IServiceCollection services)
    {
        var settings = services.BuildServiceProvider().GetService<IOptions<JwtSettings>>()?.Value ?? throw new AggregateException(nameof(JwtSettings));
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(settings.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    public static void SetAuthorizationConfiguration(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(RubelliteRoles.Administrator.GetPolicyName(), policy =>
            {
                var requiredClaims = RubelliteRoles.Administrator.GetRequiredClaim();
                policy.RequireClaim(requiredClaims.Type, requiredClaims.Value);
            });
        });
    }
    
    public static void ConfigureDatabase(this IServiceCollection services)
    {
        services.AddDbContext<RubelliteContext>((provider, options) =>
        {
            var dbSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;

            options.UseNpgsql((GetConnectionString(dbSettings)));
        }, ServiceLifetime.Scoped, ServiceLifetime.Singleton);
    }
    
    /// <summary>
    /// Register swagger gen
    /// </summary>
    /// <param name="services">service collection</param>
    public static void RegisterSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Gallphoto API v1", Version = "v1" });
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
    }
    
    private static string GetConnectionString(DatabaseSettings dbSettings)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = dbSettings.Host,
            Port = dbSettings.Port,
            Database = dbSettings.Database,
            Username = dbSettings.Username,
            Password = dbSettings.Password
        };

        return connectionStringBuilder.ConnectionString;
    }
    
    
    
}
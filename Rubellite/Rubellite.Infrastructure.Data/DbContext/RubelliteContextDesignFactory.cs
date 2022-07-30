using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Rubellite.Infrastructure.Data.DbContext;

/// <summary>
/// Fabric for applying migrations.
/// </summary>
/// <remarks>
/// environment var "ConnectionStrings__DevelopmentConnection" define a real db connection from command line 
/// export ConnectionStrings__DevelopmentConnection='Server=127.0.0.1;Port=5432;Database=RubelliteDb;User Id=RubelliteDb;Password=RubelliteDb;'
/// dotnet ef migrations add NameOfMigration --project MoneyOut.MoneyOutCost.Infrastructure
/// </remarks>
// ReSharper disable once UnusedMember.Global / используется при добавлении миграций
public class RubelliteContextDesignFactory: IDesignTimeDbContextFactory<RubelliteContext>
{
    public RubelliteContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DevelopmentConnection");
                
        var optionsBuilder = new DbContextOptionsBuilder<RubelliteContext>()
            .UseNpgsql(connectionString ?? "blank");
                
        return new RubelliteContext(optionsBuilder.Options);
    }
}
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using Rubellite.Infrastructure.Data;
using Rubellite.Infrastructure.Data.DbContext;

namespace Rubellite.WebApp.Configurations;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Накатываем миграции на базу
    /// </summary>
    /// <remarks>Метод виртуальный, т.к. нужно его переопределять в тестах</remarks>
    public static void InitializeDatabase(this IApplicationBuilder app)
    {
        var retryOnFailPolicy = Policy.Handle<Exception>().WaitAndRetry(3, _ => TimeSpan.FromSeconds(10));
        retryOnFailPolicy.Execute(app.MigrateDbContext<RubelliteContext>);
    }
    
    private static void MigrateDbContext<TContext>(this IApplicationBuilder applicationBuilder) where TContext : DbContext
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TContext>().Database;
        db.Migrate();

        using var connection = (NpgsqlConnection)db.GetDbConnection();
        connection.Open();
        connection.ReloadTypes();
    }
}
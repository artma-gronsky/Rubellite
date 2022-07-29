using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rubellite.Domain.Core;
using Rubellite.Infrastructure.Data.EntityConfigurations;

namespace Rubellite.Infrastructure.Data;

public class RubelliteContext: IdentityDbContext
{
    public RubelliteContext(DbContextOptions options) : base(options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
    }

    public DbSet<Ticket> Tickets { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}
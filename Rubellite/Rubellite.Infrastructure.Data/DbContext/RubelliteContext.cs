﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rubellite.Domain.Core;
using Rubellite.Infrastructure.Data.EntityConfigurations;

namespace Rubellite.Infrastructure.Data.DbContext;

public class RubelliteContext: IdentityDbContext<ApplicationUser>
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
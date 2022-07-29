using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rubellite.Domain.Core;

namespace Rubellite.Infrastructure.Data.EntityConfigurations;

public class TicketConfiguration: IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Ticket");

        builder.HasKey(x => x.Id);
        
        builder.Property(o => o.Id).HasDefaultValueSql("uuid_generate_v1()");

        builder.Property(o => o.Number).IsRequired();

    }
}
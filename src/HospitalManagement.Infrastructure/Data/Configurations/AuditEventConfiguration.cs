using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

public class AuditEventConfiguration : IEntityTypeConfiguration<AuditEvent>
{
    public void Configure(EntityTypeBuilder<AuditEvent> builder)
    {
        builder.ToTable("AuditEvents");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasMaxLength(450);
        builder.Property(x => x.UserName).HasMaxLength(256);
        builder.Property(x => x.Method).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Path).HasMaxLength(512).IsRequired();
        builder.Property(x => x.RemoteIpAddress).HasMaxLength(64);
        builder.HasIndex(x => x.OccurredOn);
        builder.HasIndex(x => x.UserId);
    }
}

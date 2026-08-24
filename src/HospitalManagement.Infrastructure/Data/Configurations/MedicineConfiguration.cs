using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

public class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
{
    public void Configure(EntityTypeBuilder<Medicine> builder)
    {
        builder.ToTable("Medicines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MedicineCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.MedicineName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.GenericName)
            .HasMaxLength(200);

        builder.Property(x => x.Manufacturer)
            .HasMaxLength(200);

        builder.Property(x => x.Unit)
            .HasMaxLength(50);

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(x => x.MedicineCode)
            .IsUnique();
    }
}
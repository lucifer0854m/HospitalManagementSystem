using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

public class PrescriptionItemConfiguration : IEntityTypeConfiguration<PrescriptionItem>
{
    public void Configure(EntityTypeBuilder<PrescriptionItem> builder)
    {
        builder.ToTable("PrescriptionItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Dosage)
            .HasMaxLength(100);

        builder.Property(x => x.Frequency)
            .HasMaxLength(100);

        builder.Property(x => x.Instructions)
            .HasMaxLength(500);

        builder.HasOne(x => x.Prescription)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.PrescriptionId);

        builder.HasOne(x => x.Medicine)
            .WithMany(x => x.PrescriptionItems)
            .HasForeignKey(x => x.MedicineId);
    }
}
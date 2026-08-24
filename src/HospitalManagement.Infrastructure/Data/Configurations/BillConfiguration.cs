using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalManagement.Infrastructure.Data.Configurations;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BillNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Discount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TaxAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.NetAmount)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Patient)
            .WithMany(x => x.Bills)
            .HasForeignKey(x => x.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Appointment)
            .WithOne(x => x.Bill)
            .HasForeignKey<Bill>(x => x.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.BillNumber)
            .IsUnique();
    }
}
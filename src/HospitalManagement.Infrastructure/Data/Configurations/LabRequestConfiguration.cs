using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HospitalManagement.Infrastructure.Data.Configurations;
public class LabRequestConfiguration : IEntityTypeConfiguration<LabRequest> { public void Configure(EntityTypeBuilder<LabRequest> b) { b.ToTable("LabRequests"); b.HasKey(x=>x.Id); b.Property(x=>x.RequestNumber).HasMaxLength(20).IsRequired(); b.HasIndex(x=>x.RequestNumber).IsUnique(); b.HasOne(x=>x.LabTest).WithMany(x=>x.Requests).HasForeignKey(x=>x.LabTestId).OnDelete(DeleteBehavior.Restrict); b.HasOne(x=>x.Patient).WithMany().HasForeignKey(x=>x.PatientId).OnDelete(DeleteBehavior.Restrict); b.HasOne(x=>x.Appointment).WithMany().HasForeignKey(x=>x.AppointmentId).OnDelete(DeleteBehavior.Restrict); b.HasOne(x=>x.Doctor).WithMany().HasForeignKey(x=>x.DoctorId).OnDelete(DeleteBehavior.Restrict); } }

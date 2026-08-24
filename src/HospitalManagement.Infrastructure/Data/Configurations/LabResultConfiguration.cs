using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HospitalManagement.Infrastructure.Data.Configurations;
public class LabResultConfiguration : IEntityTypeConfiguration<LabResult> { public void Configure(EntityTypeBuilder<LabResult> b) { b.ToTable("LabResults"); b.HasKey(x=>x.Id); b.Property(x=>x.ResultValue).HasMaxLength(1000).IsRequired(); b.Property(x=>x.Remarks).HasMaxLength(1000); b.HasOne(x=>x.LabRequest).WithOne(x=>x.Result).HasForeignKey<LabResult>(x=>x.LabRequestId).OnDelete(DeleteBehavior.Cascade); } }

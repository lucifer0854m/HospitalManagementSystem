using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HospitalManagement.Infrastructure.Data.Configurations;
public class LabTestConfiguration : IEntityTypeConfiguration<LabTest> { public void Configure(EntityTypeBuilder<LabTest> b) { b.ToTable("LabTests"); b.HasKey(x=>x.Id); b.Property(x=>x.TestCode).HasMaxLength(20).IsRequired(); b.Property(x=>x.TestName).HasMaxLength(200).IsRequired(); b.Property(x=>x.Category).HasMaxLength(100); b.Property(x=>x.Price).HasColumnType("decimal(18,2)"); b.Property(x=>x.NormalRange).HasMaxLength(500); b.HasIndex(x=>x.TestCode).IsUnique(); } }

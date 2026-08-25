using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Master Tables
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    // Transactions
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<PrescriptionItem> PrescriptionItems => Set<PrescriptionItem>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BillItem> BillItems => Set<BillItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<LabTest> LabTests => Set<LabTest>();
    public DbSet<LabRequest> LabRequests => Set<LabRequest>();
    public DbSet<LabResult> LabResults => Set<LabResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // We'll add Fluent API configurations here later.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

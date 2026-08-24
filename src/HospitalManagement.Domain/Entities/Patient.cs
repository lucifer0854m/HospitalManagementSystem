using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Patient : AuditableEntity
{
    // Patient Information
    public string PatientCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public BloodGroup BloodGroup { get; set; }

    // Contact Information
    public string MobileNumber { get; set; } = string.Empty;

    public string? AlternateMobileNumber { get; set; }

    public string? Email { get; set; }

    // Address
    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = "India";

    public string Pincode { get; set; } = string.Empty;

    // Emergency Contact
    public string EmergencyContactName { get; set; } = string.Empty;

    public string EmergencyContactNumber { get; set; } = string.Empty;

    public string? EmergencyContactRelation { get; set; }

    // Medical Information
    public decimal Height { get; set; }

    public decimal Weight { get; set; }

    public string? Allergies { get; set; }

    public string? MedicalHistory { get; set; }

    public PatientStatus Status { get; set; } = PatientStatus.Active;

    // Navigation Property
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
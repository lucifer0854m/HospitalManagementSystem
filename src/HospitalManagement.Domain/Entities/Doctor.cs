using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Doctor : AuditableEntity
{
    // Identification
    public string DoctorCode { get; set; } = string.Empty;

    // Personal Information
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public DateTime DateOfBirth { get; set; }

    // Professional Information
    public string Qualification { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public int ExperienceInYears { get; set; }

    public decimal ConsultationFee { get; set; }

    // Contact Information
    public string MobileNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string Address { get; set; } = string.Empty;

    // Employment
    public DateTime JoiningDate { get; set; }

    public bool IsAvailable { get; set; } = true;

    // Department Relationship
    public int DepartmentId { get; set; }

    public Department? Department { get; set; }

    // Navigation Property
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
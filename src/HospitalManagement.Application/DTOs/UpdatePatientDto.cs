using HospitalManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs;

public class UpdatePatientDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string PatientCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public BloodGroup BloodGroup { get; set; }

    [Required]
    [StringLength(15)]
    public string MobileNumber { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [StringLength(250)]
    public string? Address { get; set; }

    [Required]
    [StringLength(100)]
    public string? City { get; set; }

    [Required]
    [StringLength(100)]
    public string? State { get; set; }

    [Required]
    [StringLength(100)]
    public string? Country { get; set; } = "India";

    [Required]
    [StringLength(10)]
    public string? Pincode { get; set; }

    [StringLength(100)]
    [Required]
    public string? EmergencyContactName { get; set; }

    [StringLength(15)]
    [Required]
    public string? EmergencyContactNumber { get; set; }

    [StringLength(100)]
    public string? EmergencyContactRelation { get; set; }

    [Range(0, 999.99)]
    public decimal Height { get; set; }

    [Range(0, 999.99)]
    public decimal Weight { get; set; }

    public string? Allergies { get; set; }

    public string? MedicalHistory { get; set; }

    public PatientStatus Status { get; set; }
}

namespace HospitalManagement.Application.DTOs;

public class PatientDto
{
    public int Id { get; set; }

    public string PatientCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string BloodGroup { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? Pincode { get; set; }

    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactNumber { get; set; }

    public string? EmergencyContactRelation { get; set; }

    public decimal Height { get; set; }

    public decimal Weight { get; set; }

    public string? Allergies { get; set; }

    public string? MedicalHistory { get; set; }
}

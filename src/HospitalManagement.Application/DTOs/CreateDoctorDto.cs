using HospitalManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs;

public class CreateDoctorDto
{
    [Required, StringLength(20)] public string DoctorCode { get; set; } = string.Empty;
    [Required, StringLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required, StringLength(100)] public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    [Required] public DateTime DateOfBirth { get; set; }
    [Required, StringLength(100)] public string Qualification { get; set; } = string.Empty;
    [Required, StringLength(100)] public string Specialization { get; set; } = string.Empty;
    [Range(0, 80)] public int ExperienceInYears { get; set; }
    [Range(0, 999999.99)] public decimal ConsultationFee { get; set; }
    [Required, StringLength(15)] public string MobileNumber { get; set; } = string.Empty;
    [EmailAddress] public string? Email { get; set; }
    [Required, StringLength(250)] public string Address { get; set; } = string.Empty;
    [Required] public DateTime JoiningDate { get; set; } = DateTime.Today;
    public bool IsAvailable { get; set; } = true;
    [Range(1, int.MaxValue)] public int DepartmentId { get; set; }
}

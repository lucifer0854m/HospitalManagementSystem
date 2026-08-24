using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs;

public class CreateDepartmentDto
{
    [Required, StringLength(20)] public string DepartmentCode { get; set; } = string.Empty;
    [Required, StringLength(100)] public string Name { get; set; } = string.Empty;
    [StringLength(500)] public string? Description { get; set; }
    [StringLength(100)] public string? Location { get; set; }
    public bool IsActive { get; set; } = true;
}

namespace HospitalManagement.Application.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }
    public string DepartmentCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
}

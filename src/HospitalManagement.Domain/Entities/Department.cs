using HospitalManagement.Domain.Common;

namespace HospitalManagement.Domain.Entities;

public class Department : AuditableEntity
{
    public string DepartmentCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Property
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
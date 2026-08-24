using HospitalManagement.Domain.Common;
namespace HospitalManagement.Domain.Entities;
public class LabTest : AuditableEntity { public string TestCode { get; set; } = string.Empty; public string TestName { get; set; } = string.Empty; public string? Category { get; set; } public decimal Price { get; set; } public string? NormalRange { get; set; } public bool IsActive { get; set; } = true; public ICollection<LabRequest> Requests { get; set; } = new List<LabRequest>(); }

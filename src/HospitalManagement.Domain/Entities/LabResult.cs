using HospitalManagement.Domain.Common;
namespace HospitalManagement.Domain.Entities;
public class LabResult : AuditableEntity { public int LabRequestId { get; set; } public LabRequest? LabRequest { get; set; } public string ResultValue { get; set; } = string.Empty; public string? Remarks { get; set; } public DateTime ReportedOn { get; set; } }

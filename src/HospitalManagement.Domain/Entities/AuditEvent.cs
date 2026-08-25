using HospitalManagement.Domain.Common;

namespace HospitalManagement.Domain.Entities;

public class AuditEvent : BaseEntity
{
    public DateTime OccurredOn { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string? RemoteIpAddress { get; set; }
}

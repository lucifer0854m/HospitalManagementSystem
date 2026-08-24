using HospitalManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs;

public class CreateAppointmentDto
{
    [Required, StringLength(20)] public string AppointmentNumber { get; set; } = string.Empty;
    [Range(1, int.MaxValue)] public int PatientId { get; set; }
    [Range(1, int.MaxValue)] public int DoctorId { get; set; }
    [Required] public DateTime AppointmentDate { get; set; } = DateTime.Today;
    [Required] public TimeSpan AppointmentTime { get; set; }
    public AppointmentType AppointmentType { get; set; } = AppointmentType.OPD;
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    [Required, StringLength(500)] public string Reason { get; set; } = string.Empty;
    [StringLength(1000)] public string? Symptoms { get; set; }
    [StringLength(2000)] public string? Notes { get; set; }
    public bool IsFollowUp { get; set; }
    public DateTime? FollowUpDate { get; set; }
}

using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Appointment : AuditableEntity
{
    public string AppointmentNumber { get; set; } = string.Empty;

    // Patient
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    // Doctor
    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    // Appointment
    public DateTime AppointmentDate { get; set; }

    public TimeSpan AppointmentTime { get; set; }

    public AppointmentType AppointmentType { get; set; }

    public AppointmentStatus Status { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string? Symptoms { get; set; }

    public string? Diagnosis { get; set; }

    public string? PrescriptionNote { get; set; }

    public string? Notes { get; set; }

    public bool IsFollowUp { get; set; }

    public DateTime? FollowUpDate { get; set; }

    public Bill? Bill { get; set; }

    public Prescription? Prescription { get; set; }
    
}
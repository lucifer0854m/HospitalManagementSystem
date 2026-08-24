using HospitalManagement.Domain.Common;

namespace HospitalManagement.Domain.Entities;

public class Prescription : AuditableEntity
{
    public int AppointmentId { get; set; }

    public Appointment? Appointment { get; set; }

    public int PatientId { get; set; }

    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }

    public Doctor? Doctor { get; set; }

    public DateTime PrescriptionDate { get; set; }

    public string? Notes { get; set; }

    // Navigation Property
    public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
}
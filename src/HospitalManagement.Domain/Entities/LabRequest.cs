using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;
namespace HospitalManagement.Domain.Entities;
public class LabRequest : AuditableEntity { public string RequestNumber { get; set; } = string.Empty; public int LabTestId { get; set; } public LabTest? LabTest { get; set; } public int PatientId { get; set; } public Patient? Patient { get; set; } public int? AppointmentId { get; set; } public Appointment? Appointment { get; set; } public int? DoctorId { get; set; } public Doctor? Doctor { get; set; } public DateTime RequestedOn { get; set; } public LabRequestStatus Status { get; set; } = LabRequestStatus.Ordered; public LabResult? Result { get; set; } }

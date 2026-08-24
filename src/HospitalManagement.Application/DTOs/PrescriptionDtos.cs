using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs;

public class PrescriptionLineDto { [Range(1, int.MaxValue)] public int MedicineId { get; set; } [Required, StringLength(100)] public string Dosage { get; set; } = string.Empty; [Required, StringLength(100)] public string Frequency { get; set; } = string.Empty; [Range(1, 3650)] public int DurationInDays { get; set; } public string? Instructions { get; set; } }
public class CreatePrescriptionDto { [Range(1, int.MaxValue)] public int AppointmentId { get; set; } [Range(1, int.MaxValue)] public int PatientId { get; set; } [Range(1, int.MaxValue)] public int DoctorId { get; set; } public DateTime PrescriptionDate { get; set; } = DateTime.Today; [StringLength(1000)] public string? Notes { get; set; } [MinLength(1)] public List<PrescriptionLineDto> Items { get; set; } = []; }

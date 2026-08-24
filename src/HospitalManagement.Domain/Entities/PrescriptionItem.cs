using HospitalManagement.Domain.Common;

namespace HospitalManagement.Domain.Entities;

public class PrescriptionItem : BaseEntity
{
    public int PrescriptionId { get; set; }

    public Prescription? Prescription { get; set; }

    public int MedicineId { get; set; }

    public Medicine? Medicine { get; set; }

    public string Dosage { get; set; } = string.Empty;

    public string Frequency { get; set; } = string.Empty;

    public int DurationInDays { get; set; }

    public string? Instructions { get; set; }


}
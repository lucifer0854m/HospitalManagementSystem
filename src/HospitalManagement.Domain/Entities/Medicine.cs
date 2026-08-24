using HospitalManagement.Domain.Common;

namespace HospitalManagement.Domain.Entities;

public class Medicine : AuditableEntity
{
    public string MedicineCode { get; set; } = string.Empty;

    public string MedicineName { get; set; } = string.Empty;

    public string GenericName { get; set; } = string.Empty;

    public string Manufacturer { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Property
    public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
}
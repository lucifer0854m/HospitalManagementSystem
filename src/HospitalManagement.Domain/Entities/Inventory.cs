using HospitalManagement.Domain.Common;

namespace HospitalManagement.Domain.Entities;

public class Inventory : AuditableEntity
{
    public int MedicineId { get; set; }

    public Medicine? Medicine { get; set; }

    public int AvailableQuantity { get; set; }

    public int ReorderLevel { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? BatchNumber { get; set; }

    public string? SupplierName { get; set; }
}
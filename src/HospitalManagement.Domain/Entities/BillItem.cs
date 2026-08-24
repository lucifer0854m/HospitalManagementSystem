using HospitalManagement.Domain.Common;

namespace HospitalManagement.Domain.Entities;

public class BillItem : BaseEntity
{
    public int BillId { get; set; }

    public Bill? Bill { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
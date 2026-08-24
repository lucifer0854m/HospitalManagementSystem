using HospitalManagement.Domain.Common;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Payment : AuditableEntity
{
    public int BillId { get; set; }

    public Bill? Bill { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string? TransactionReference { get; set; }

    public string? Remarks { get; set; }
}
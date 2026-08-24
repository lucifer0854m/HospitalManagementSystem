using HospitalManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs;

public class BillLineDto { [Required, StringLength(200)] public string ItemName { get; set; } = string.Empty; [Range(1, int.MaxValue)] public int Quantity { get; set; } = 1; [Range(0, 999999.99)] public decimal UnitPrice { get; set; } }
public class CreateBillDto { [Required, StringLength(20)] public string BillNumber { get; set; } = string.Empty; [Range(1, int.MaxValue)] public int PatientId { get; set; } public int? AppointmentId { get; set; } public DateTime BillDate { get; set; } = DateTime.Today; [Range(0, 999999.99)] public decimal Discount { get; set; } [Range(0, 999999.99)] public decimal TaxAmount { get; set; } [MinLength(1)] public List<BillLineDto> Items { get; set; } = []; }
public class RecordPaymentDto { [Range(1, int.MaxValue)] public int BillId { get; set; } [Range(0.01, 999999.99)] public decimal Amount { get; set; } public DateTime PaymentDate { get; set; } = DateTime.Today; public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash; [StringLength(100)] public string? TransactionReference { get; set; } [StringLength(500)] public string? Remarks { get; set; } }
public class BillSummaryDto { public int Id { get; set; } public string BillNumber { get; set; } = string.Empty; public decimal NetAmount { get; set; } public decimal PaidAmount { get; set; } public decimal Balance { get; set; } public PaymentStatus PaymentStatus { get; set; } }

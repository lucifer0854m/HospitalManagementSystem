using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Web.Models;

public class BillingIndexViewModel
{
    public CreateBillDto NewBill { get; set; } = new() { Items = [new BillLineDto()] };
    public RecordPaymentDto NewPayment { get; set; } = new();
    public IEnumerable<BillListDto> Bills { get; set; } = [];
}

public class PharmacyIndexViewModel
{
    public IEnumerable<MedicineDto> Medicines { get; set; } = [];
    public IEnumerable<InventoryDto> Inventory { get; set; } = [];
    public IEnumerable<InventoryDto> LowStock { get; set; } = [];
}

public class LaboratoryIndexViewModel
{
    public IEnumerable<LabTestDto> Tests { get; set; } = [];
    public IEnumerable<LabRequestDto> Requests { get; set; } = [];
}

using HospitalManagement.Application.DTOs;
namespace HospitalManagement.Application.Interfaces;
public interface IBillingService { Task<IEnumerable<BillListDto>> GetBillsAsync(); Task<int> CreateBillAsync(CreateBillDto dto); Task<BillSummaryDto> RecordPaymentAsync(RecordPaymentDto dto); }

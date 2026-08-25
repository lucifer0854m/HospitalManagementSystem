using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;
public class BillingService : IBillingService
{
    private readonly IGenericRepository<Bill> _bills; private readonly IGenericRepository<BillItem> _billItems; private readonly IGenericRepository<Payment> _payments; private readonly IGenericRepository<Patient> _patients; private readonly IGenericRepository<Appointment> _appointments;
    public BillingService(IGenericRepository<Bill> bills, IGenericRepository<BillItem> billItems, IGenericRepository<Payment> payments, IGenericRepository<Patient> patients, IGenericRepository<Appointment> appointments) => (_bills,_billItems,_payments,_patients,_appointments)=(bills,billItems,payments,patients,appointments);
    public async Task<IEnumerable<BillListDto>> GetBillsAsync()
    {
        var patients = (await _patients.GetAllAsync()).ToDictionary(x => x.Id, x => $"{x.FirstName} {x.LastName}".Trim());
        var payments = await _payments.GetAllAsync();
        return (await _bills.GetAllAsync()).OrderByDescending(x => x.BillDate).ThenByDescending(x => x.Id).Select(x => new BillListDto { Id=x.Id, BillNumber=x.BillNumber, PatientId=x.PatientId, PatientName=patients.GetValueOrDefault(x.PatientId, "Unknown"), BillDate=x.BillDate, NetAmount=x.NetAmount, PaidAmount=payments.Where(p=>p.BillId==x.Id).Sum(p=>p.Amount), Balance=x.NetAmount-payments.Where(p=>p.BillId==x.Id).Sum(p=>p.Amount), PaymentStatus=x.PaymentStatus });
    }
    public async Task<int> CreateBillAsync(CreateBillDto dto)
    {
        if (!await _patients.ExistsAsync(dto.PatientId)) throw new ArgumentException("Select a valid patient."); if (dto.AppointmentId.HasValue && !await _appointments.ExistsAsync(dto.AppointmentId.Value)) throw new ArgumentException("Select a valid appointment.");
        if ((await _bills.FindAsync(x=>x.BillNumber==dto.BillNumber.Trim())).Any()) throw new InvalidOperationException("A bill with this number already exists.");
        var total=dto.Items.Sum(x=>x.Quantity*x.UnitPrice); if(dto.Discount>total+dto.TaxAmount) throw new ArgumentException("Discount cannot exceed the bill total.");
        var bill=new Bill{BillNumber=dto.BillNumber.Trim(),PatientId=dto.PatientId,AppointmentId=dto.AppointmentId,BillDate=dto.BillDate,TotalAmount=total,Discount=dto.Discount,TaxAmount=dto.TaxAmount,NetAmount=total-dto.Discount+dto.TaxAmount,PaymentStatus=PaymentStatus.Pending,CreatedOn=DateTime.UtcNow}; await _bills.AddAsync(bill); await _bills.SaveChangesAsync();
        foreach(var line in dto.Items) await _billItems.AddAsync(new BillItem{BillId=bill.Id,ItemName=line.ItemName.Trim(),Quantity=line.Quantity,UnitPrice=line.UnitPrice,TotalPrice=line.Quantity*line.UnitPrice}); await _billItems.SaveChangesAsync(); return bill.Id;
    }
    public async Task<BillSummaryDto> RecordPaymentAsync(RecordPaymentDto dto)
    {
        var bill=await _bills.GetByIdAsync(dto.BillId)??throw new KeyNotFoundException("Bill not found."); var paid=(await _payments.FindAsync(x=>x.BillId==bill.Id)).Sum(x=>x.Amount); if(dto.Amount>bill.NetAmount-paid) throw new ArgumentException("Payment amount exceeds the outstanding balance.");
        await _payments.AddAsync(new Payment{BillId=bill.Id,Amount=dto.Amount,PaymentDate=dto.PaymentDate,PaymentMethod=dto.PaymentMethod,TransactionReference=dto.TransactionReference,Remarks=dto.Remarks,CreatedOn=DateTime.UtcNow}); await _payments.SaveChangesAsync(); paid+=dto.Amount; bill.PaymentStatus=paid>=bill.NetAmount?PaymentStatus.Paid:PaymentStatus.Partial; bill.ModifiedOn=DateTime.UtcNow; _bills.Update(bill); await _bills.SaveChangesAsync(); return new BillSummaryDto{Id=bill.Id,BillNumber=bill.BillNumber,NetAmount=bill.NetAmount,PaidAmount=paid,Balance=bill.NetAmount-paid,PaymentStatus=bill.PaymentStatus};
    }
}

using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces;
namespace HospitalManagement.Application.Services;
public class ReportingService : IReportingService
{
    private readonly IGenericRepository<Patient> _patients; private readonly IGenericRepository<Doctor> _doctors; private readonly IGenericRepository<Appointment> _appointments; private readonly IGenericRepository<LabRequest> _labRequests; private readonly IGenericRepository<Bill> _bills; private readonly IGenericRepository<Payment> _payments;
    public ReportingService(IGenericRepository<Patient> patients,IGenericRepository<Doctor> doctors,IGenericRepository<Appointment> appointments,IGenericRepository<LabRequest> labRequests,IGenericRepository<Bill> bills,IGenericRepository<Payment> payments)=>(_patients,_doctors,_appointments,_labRequests,_bills,_payments)=(patients,doctors,appointments,labRequests,bills,payments);
    public async Task<DashboardDto> GetDashboardAsync(){var today=DateTime.UtcNow.Date;return new DashboardDto{TotalPatients=(await _patients.GetAllAsync()).Count(),TotalDoctors=(await _doctors.GetAllAsync()).Count(),TodayAppointments=(await _appointments.FindAsync(x=>x.AppointmentDate.Date==today)).Count(),PendingLabRequests=(await _labRequests.FindAsync(x=>x.Status!=LabRequestStatus.Completed&&x.Status!=LabRequestStatus.Cancelled)).Count(),TodayRevenue=(await _payments.FindAsync(x=>x.PaymentDate.Date==today)).Sum(x=>x.Amount)};}
    public async Task<ReportDto> GetReportAsync(DateTime from,DateTime to){if(to.Date<from.Date)throw new ArgumentException("The end date must be on or after the start date.");var appointments=await _appointments.FindAsync(x=>x.AppointmentDate.Date>=from.Date&&x.AppointmentDate.Date<=to.Date);var bills=await _bills.FindAsync(x=>x.BillDate.Date>=from.Date&&x.BillDate.Date<=to.Date);var payments=await _payments.FindAsync(x=>x.PaymentDate.Date>=from.Date&&x.PaymentDate.Date<=to.Date);return new ReportDto{From=from.Date,To=to.Date,Appointments=appointments.Count(),CompletedAppointments=appointments.Count(x=>x.Status==AppointmentStatus.Completed),LabRequests=(await _labRequests.FindAsync(x=>x.RequestedOn.Date>=from.Date&&x.RequestedOn.Date<=to.Date)).Count(),BilledAmount=bills.Sum(x=>x.NetAmount),PaidAmount=payments.Sum(x=>x.Amount)};}
}

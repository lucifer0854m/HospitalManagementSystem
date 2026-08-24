using HospitalManagement.Application.DTOs;
namespace HospitalManagement.Application.Interfaces;
public interface IReportingService { Task<DashboardDto> GetDashboardAsync(); Task<ReportDto> GetReportAsync(DateTime from,DateTime to); }

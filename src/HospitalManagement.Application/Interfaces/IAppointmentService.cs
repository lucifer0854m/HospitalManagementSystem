using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAsync();
    Task<AppointmentDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateAppointmentDto dto);
    Task UpdateAsync(UpdateAppointmentDto dto);
    Task DeleteAsync(int id);
}

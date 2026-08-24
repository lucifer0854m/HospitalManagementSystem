using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllAsync();
    Task<DoctorDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateDoctorDto dto);
    Task UpdateAsync(UpdateDoctorDto dto);
    Task DeleteAsync(int id);
}

using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync();

    Task<PatientDto?> GetByIdAsync(int id);

    Task CreateAsync(CreatePatientDto dto);

    Task UpdateAsync(UpdatePatientDto dto);

    Task DeleteAsync(int id);
}
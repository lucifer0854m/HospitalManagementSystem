using HospitalManagement.Application.DTOs;
namespace HospitalManagement.Application.Interfaces;
public interface IPrescriptionService
{
    Task<int> CreatePrescriptionAsync(CreatePrescriptionDto dto);
    Task<IEnumerable<PrescriptionListDto>> GetAllAsync();
    Task<PrescriptionDetailsDto?> GetByIdAsync(int id);
}

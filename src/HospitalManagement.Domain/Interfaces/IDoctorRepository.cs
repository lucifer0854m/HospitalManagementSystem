using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<Doctor?> GetByDoctorCodeAsync(string doctorCode);

    Task<IEnumerable<Doctor>> GetAllWithDepartmentAsync();

    Task<Doctor?> GetWithDepartmentAsync(int id);
}

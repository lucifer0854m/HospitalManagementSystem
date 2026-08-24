using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<Patient?> GetByPatientCodeAsync(string patientCode);

    Task<IEnumerable<Patient>> SearchAsync(string keyword);
}
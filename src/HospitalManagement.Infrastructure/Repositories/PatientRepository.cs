using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class PatientRepository
    : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Patient?> GetByPatientCodeAsync(string patientCode)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(x => x.PatientCode == patientCode);
    }

    public async Task<IEnumerable<Patient>> SearchAsync(string keyword)
    {
        return await _context.Patients
            .Where(x =>
                x.FirstName.Contains(keyword) ||
                x.LastName.Contains(keyword) ||
                x.PatientCode.Contains(keyword))
            .ToListAsync();
    }
}
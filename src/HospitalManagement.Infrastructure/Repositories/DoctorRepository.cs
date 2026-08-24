using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ApplicationDbContext context) : base(context) { }

    public Task<Doctor?> GetByDoctorCodeAsync(string doctorCode) =>
        _context.Doctors.FirstOrDefaultAsync(x => x.DoctorCode == doctorCode);

    public async Task<IEnumerable<Doctor>> GetAllWithDepartmentAsync() =>
        await _context.Doctors.Include(x => x.Department).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToListAsync();

    public Task<Doctor?> GetWithDepartmentAsync(int id) =>
        _context.Doctors.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
}

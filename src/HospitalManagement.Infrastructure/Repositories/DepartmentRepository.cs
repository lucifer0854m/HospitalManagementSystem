using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context) : base(context) { }
    public Task<Department?> GetByDepartmentCodeAsync(string departmentCode) => _context.Departments.FirstOrDefaultAsync(x => x.DepartmentCode == departmentCode);
}

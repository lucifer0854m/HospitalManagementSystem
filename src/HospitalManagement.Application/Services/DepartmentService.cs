using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IGenericRepository<Doctor> _doctorRepository;
    private readonly IMapper _mapper;
    public DepartmentService(IDepartmentRepository departmentRepository, IGenericRepository<Doctor> doctorRepository, IMapper mapper) => (_departmentRepository, _doctorRepository, _mapper) = (departmentRepository, doctorRepository, mapper);
    public async Task<IEnumerable<DepartmentDto>> GetAllAsync() => _mapper.Map<IEnumerable<DepartmentDto>>((await _departmentRepository.GetAllAsync()).OrderBy(x => x.Name));
    public async Task<DepartmentDto?> GetByIdAsync(int id) { var department = await _departmentRepository.GetByIdAsync(id); return department is null ? null : _mapper.Map<DepartmentDto>(department); }
    public async Task CreateAsync(CreateDepartmentDto dto)
    {
        await ValidateCodeAsync(dto.DepartmentCode);
        var department = _mapper.Map<Department>(dto);
        department.DepartmentCode = dto.DepartmentCode.Trim(); department.Name = dto.Name.Trim(); department.CreatedOn = DateTime.UtcNow;
        await _departmentRepository.AddAsync(department); await _departmentRepository.SaveChangesAsync();
    }
    public async Task UpdateAsync(UpdateDepartmentDto dto)
    {
        var department = await _departmentRepository.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException("Department not found.");
        await ValidateCodeAsync(dto.DepartmentCode, dto.Id);
        _mapper.Map(dto, department); department.DepartmentCode = dto.DepartmentCode.Trim(); department.Name = dto.Name.Trim(); department.ModifiedOn = DateTime.UtcNow;
        _departmentRepository.Update(department); await _departmentRepository.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Department not found.");
        if ((await _doctorRepository.FindAsync(x => x.DepartmentId == id)).Any())
            throw new InvalidOperationException("A department with assigned doctors cannot be deleted.");
        _departmentRepository.Delete(department); await _departmentRepository.SaveChangesAsync();
    }
    private async Task ValidateCodeAsync(string code, int? currentId = null)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Department code is required.");
        var existing = await _departmentRepository.GetByDepartmentCodeAsync(code.Trim());
        if (existing is not null && existing.Id != currentId) throw new InvalidOperationException("A department with this code already exists.");
    }
}

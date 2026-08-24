using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IGenericRepository<Department> _departmentRepository;
    private readonly IMapper _mapper;

    public DoctorService(IDoctorRepository doctorRepository, IGenericRepository<Department> departmentRepository, IMapper mapper)
    {
        _doctorRepository = doctorRepository;
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<DoctorDto>>(await _doctorRepository.GetAllWithDepartmentAsync());

    public async Task<DoctorDto?> GetByIdAsync(int id)
    {
        var doctor = await _doctorRepository.GetWithDepartmentAsync(id);
        return doctor is null ? null : _mapper.Map<DoctorDto>(doctor);
    }

    public async Task CreateAsync(CreateDoctorDto dto)
    {
        await ValidateDoctorAsync(dto.DoctorCode, dto.DateOfBirth, dto.JoiningDate, dto.DepartmentId);
        var doctor = _mapper.Map<Doctor>(dto);
        doctor.DoctorCode = dto.DoctorCode.Trim();
        doctor.FirstName = dto.FirstName.Trim();
        doctor.LastName = dto.LastName.Trim();
        doctor.CreatedOn = DateTime.UtcNow;
        await _doctorRepository.AddAsync(doctor);
        await _doctorRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateDoctorDto dto)
    {
        var doctor = await _doctorRepository.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException("Doctor not found.");
        await ValidateDoctorAsync(dto.DoctorCode, dto.DateOfBirth, dto.JoiningDate, dto.DepartmentId, dto.Id);
        _mapper.Map(dto, doctor);
        doctor.DoctorCode = dto.DoctorCode.Trim();
        doctor.FirstName = dto.FirstName.Trim();
        doctor.LastName = dto.LastName.Trim();
        doctor.ModifiedOn = DateTime.UtcNow;
        _doctorRepository.Update(doctor);
        await _doctorRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Doctor not found.");
        _doctorRepository.Delete(doctor);
        await _doctorRepository.SaveChangesAsync();
    }

    private async Task ValidateDoctorAsync(string doctorCode, DateTime dateOfBirth, DateTime joiningDate, int departmentId, int? currentDoctorId = null)
    {
        if (string.IsNullOrWhiteSpace(doctorCode)) throw new ArgumentException("Doctor code is required.");
        if (dateOfBirth.Date > DateTime.UtcNow.Date) throw new ArgumentException("Date of birth cannot be in the future.");
        if (joiningDate.Date > DateTime.UtcNow.Date) throw new ArgumentException("Joining date cannot be in the future.");
        if (!await _departmentRepository.ExistsAsync(departmentId)) throw new ArgumentException("Select a valid department.");

        var existingDoctor = await _doctorRepository.GetByDoctorCodeAsync(doctorCode.Trim());
        if (existingDoctor is not null && existingDoctor.Id != currentDoctorId)
            throw new InvalidOperationException("A doctor with this doctor code already exists.");
    }
}

using HospitalManagement.Domain.Entities;
using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public PatientService(
        IPatientRepository patientRepository,
        IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync()
    {
        var patients = await _patientRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<PatientDto>>(patients);
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
            return null;

        return _mapper.Map<PatientDto>(patient);
    }

    public async Task CreateAsync(CreatePatientDto dto)
    {
        var patient = _mapper.Map<Patient>(dto);

        await _patientRepository.AddAsync(patient);

        await _patientRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdatePatientDto dto)
    {
        var patient = await _patientRepository.GetByIdAsync(dto.Id);

        if (patient == null)
            throw new Exception("Patient not found.");

        _mapper.Map(dto, patient);

        _patientRepository.Update(patient);

        await _patientRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
            throw new Exception("Patient not found.");

        _patientRepository.Delete(patient);

        await _patientRepository.SaveChangesAsync();
    }
}
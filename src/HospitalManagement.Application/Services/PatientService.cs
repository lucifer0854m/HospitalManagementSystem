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
        await ValidatePatientAsync(dto.PatientCode, dto.DateOfBirth);

        var patient = _mapper.Map<Patient>(dto);
        patient.PatientCode = dto.PatientCode.Trim();
        patient.FirstName = dto.FirstName.Trim();
        patient.LastName = dto.LastName.Trim();
        patient.CreatedOn = DateTime.UtcNow;

        await _patientRepository.AddAsync(patient);

        await _patientRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdatePatientDto dto)
    {
        var patient = await _patientRepository.GetByIdAsync(dto.Id);

        if (patient == null)
            throw new KeyNotFoundException("Patient not found.");

        await ValidatePatientAsync(dto.PatientCode, dto.DateOfBirth, dto.Id);

        _mapper.Map(dto, patient);
        patient.PatientCode = dto.PatientCode.Trim();
        patient.FirstName = dto.FirstName.Trim();
        patient.LastName = dto.LastName.Trim();
        patient.ModifiedOn = DateTime.UtcNow;

        _patientRepository.Update(patient);

        await _patientRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
            throw new KeyNotFoundException("Patient not found.");

        _patientRepository.Delete(patient);

        await _patientRepository.SaveChangesAsync();
    }

    private async Task ValidatePatientAsync(string patientCode, DateTime dateOfBirth, int? currentPatientId = null)
    {
        if (string.IsNullOrWhiteSpace(patientCode))
            throw new ArgumentException("Patient code is required.");

        if (dateOfBirth.Date > DateTime.UtcNow.Date)
            throw new ArgumentException("Date of birth cannot be in the future.");

        var existingPatient = await _patientRepository.GetByPatientCodeAsync(patientCode.Trim());
        if (existingPatient is not null && existingPatient.Id != currentPatientId)
            throw new InvalidOperationException("A patient with this patient code already exists.");
    }
}

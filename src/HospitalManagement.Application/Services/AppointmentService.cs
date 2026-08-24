using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IGenericRepository<Patient> _patientRepository;
    private readonly IGenericRepository<Doctor> _doctorRepository;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository appointmentRepository, IGenericRepository<Patient> patientRepository, IGenericRepository<Doctor> doctorRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync() => _mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentRepository.GetAllWithReferencesAsync());
    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        var appointment = await _appointmentRepository.GetWithReferencesAsync(id);
        return appointment is null ? null : _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task CreateAsync(CreateAppointmentDto dto)
    {
        await ValidateAppointmentAsync(dto);
        var appointment = _mapper.Map<Appointment>(dto);
        appointment.AppointmentNumber = dto.AppointmentNumber.Trim();
        appointment.CreatedOn = DateTime.UtcNow;
        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateAppointmentDto dto)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException("Appointment not found.");
        await ValidateAppointmentAsync(dto, dto.Id);
        _mapper.Map(dto, appointment);
        appointment.AppointmentNumber = dto.AppointmentNumber.Trim();
        appointment.ModifiedOn = DateTime.UtcNow;
        _appointmentRepository.Update(appointment);
        await _appointmentRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Appointment not found.");
        _appointmentRepository.Delete(appointment);
        await _appointmentRepository.SaveChangesAsync();
    }

    private async Task ValidateAppointmentAsync(CreateAppointmentDto dto, int? currentAppointmentId = null)
    {
        if (string.IsNullOrWhiteSpace(dto.AppointmentNumber)) throw new ArgumentException("Appointment number is required.");
        if (dto.AppointmentDate.Date < DateTime.UtcNow.Date) throw new ArgumentException("Appointments cannot be scheduled in the past.");
        if (dto.IsFollowUp && (!dto.FollowUpDate.HasValue || dto.FollowUpDate.Value.Date < dto.AppointmentDate.Date)) throw new ArgumentException("Follow-up date must be on or after the appointment date.");
        if (!await _patientRepository.ExistsAsync(dto.PatientId)) throw new ArgumentException("Select a valid patient.");
        if (!await _doctorRepository.ExistsAsync(dto.DoctorId)) throw new ArgumentException("Select a valid doctor.");
        if (await _appointmentRepository.HasSchedulingConflictAsync(dto.DoctorId, dto.AppointmentDate, dto.AppointmentTime, currentAppointmentId)) throw new InvalidOperationException("The doctor already has an appointment at this date and time.");
        var existing = await _appointmentRepository.GetByAppointmentNumberAsync(dto.AppointmentNumber.Trim());
        if (existing is not null && existing.Id != currentAppointmentId) throw new InvalidOperationException("An appointment with this number already exists.");
    }
}

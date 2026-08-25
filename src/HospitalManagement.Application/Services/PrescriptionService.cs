using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;
public class PrescriptionService : IPrescriptionService
{
    private readonly IGenericRepository<Prescription> _prescriptions; private readonly IGenericRepository<PrescriptionItem> _items; private readonly IGenericRepository<Appointment> _appointments; private readonly IGenericRepository<Patient> _patients; private readonly IGenericRepository<Doctor> _doctors; private readonly IGenericRepository<Medicine> _medicines;
    public PrescriptionService(IGenericRepository<Prescription> prescriptions, IGenericRepository<PrescriptionItem> items, IGenericRepository<Appointment> appointments, IGenericRepository<Patient> patients, IGenericRepository<Doctor> doctors, IGenericRepository<Medicine> medicines) => (_prescriptions,_items,_appointments,_patients,_doctors,_medicines)=(prescriptions,items,appointments,patients,doctors,medicines);
    public async Task<int> CreatePrescriptionAsync(CreatePrescriptionDto dto)
    {
        if (dto.Items.Count == 0) throw new ArgumentException("Add at least one medicine.");
        var appointment = await _appointments.GetByIdAsync(dto.AppointmentId);
        if (appointment is null || !await _patients.ExistsAsync(dto.PatientId) || !await _doctors.ExistsAsync(dto.DoctorId)) throw new ArgumentException("Select valid appointment, patient, and doctor records.");
        if (appointment.PatientId != dto.PatientId || appointment.DoctorId != dto.DoctorId) throw new ArgumentException("The selected patient and doctor must match the appointment.");
        foreach(var item in dto.Items) if(!await _medicines.ExistsAsync(item.MedicineId)) throw new ArgumentException("One or more selected medicines are invalid.");
        var prescription=new Prescription{AppointmentId=dto.AppointmentId,PatientId=dto.PatientId,DoctorId=dto.DoctorId,PrescriptionDate=dto.PrescriptionDate,Notes=dto.Notes,CreatedOn=DateTime.UtcNow}; await _prescriptions.AddAsync(prescription); await _prescriptions.SaveChangesAsync(); foreach(var line in dto.Items) await _items.AddAsync(new PrescriptionItem{PrescriptionId=prescription.Id,MedicineId=line.MedicineId,Dosage=line.Dosage,Frequency=line.Frequency,DurationInDays=line.DurationInDays,Instructions=line.Instructions}); await _items.SaveChangesAsync(); return prescription.Id;
    }

    public async Task<IEnumerable<PrescriptionListDto>> GetAllAsync()
    {
        var prescriptions = (await _prescriptions.GetAllAsync()).OrderByDescending(x => x.PrescriptionDate).ToList();
        var appointments = (await _appointments.GetAllAsync()).ToDictionary(x => x.Id);
        var patients = (await _patients.GetAllAsync()).ToDictionary(x => x.Id);
        var doctors = (await _doctors.GetAllAsync()).ToDictionary(x => x.Id);
        var itemCounts = (await _items.GetAllAsync()).GroupBy(x => x.PrescriptionId).ToDictionary(x => x.Key, x => x.Count());

        return prescriptions.Select(x => ToListDto(x, appointments, patients, doctors, itemCounts));
    }

    public async Task<PrescriptionDetailsDto?> GetByIdAsync(int id)
    {
        var prescription = await _prescriptions.GetByIdAsync(id);
        if (prescription is null) return null;

        var appointments = (await _appointments.GetAllAsync()).ToDictionary(x => x.Id);
        var patients = (await _patients.GetAllAsync()).ToDictionary(x => x.Id);
        var doctors = (await _doctors.GetAllAsync()).ToDictionary(x => x.Id);
        var items = (await _items.FindAsync(x => x.PrescriptionId == id)).ToList();
        var medicines = (await _medicines.GetAllAsync()).ToDictionary(x => x.Id);
        var summary = ToListDto(prescription, appointments, patients, doctors, new Dictionary<int, int> { [id] = items.Count });

        return new PrescriptionDetailsDto
        {
            Id = summary.Id, AppointmentId = summary.AppointmentId, AppointmentNumber = summary.AppointmentNumber,
            PatientName = summary.PatientName, DoctorName = summary.DoctorName, PrescriptionDate = summary.PrescriptionDate,
            ItemCount = summary.ItemCount, Notes = prescription.Notes,
            Items = items.Select(x => new PrescriptionItemDto
            {
                MedicineName = medicines.GetValueOrDefault(x.MedicineId)?.MedicineName ?? "Unknown medicine",
                Dosage = x.Dosage, Frequency = x.Frequency, DurationInDays = x.DurationInDays, Instructions = x.Instructions
            })
        };
    }

    private static PrescriptionListDto ToListDto(Prescription prescription, IReadOnlyDictionary<int, Appointment> appointments, IReadOnlyDictionary<int, Patient> patients, IReadOnlyDictionary<int, Doctor> doctors, IReadOnlyDictionary<int, int> itemCounts) => new()
    {
        Id = prescription.Id, AppointmentId = prescription.AppointmentId,
        AppointmentNumber = appointments.GetValueOrDefault(prescription.AppointmentId)?.AppointmentNumber ?? "Unknown appointment",
        PatientName = patients.GetValueOrDefault(prescription.PatientId) is { } patient ? $"{patient.FirstName} {patient.LastName}" : "Unknown patient",
        DoctorName = doctors.GetValueOrDefault(prescription.DoctorId) is { } doctor ? $"Dr. {doctor.FirstName} {doctor.LastName}" : "Unknown doctor",
        PrescriptionDate = prescription.PrescriptionDate, ItemCount = itemCounts.GetValueOrDefault(prescription.Id)
    };
}

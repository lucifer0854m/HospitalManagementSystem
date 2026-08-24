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
        if(!await _appointments.ExistsAsync(dto.AppointmentId)||!await _patients.ExistsAsync(dto.PatientId)||!await _doctors.ExistsAsync(dto.DoctorId)) throw new ArgumentException("Select valid appointment, patient, and doctor records."); foreach(var item in dto.Items) if(!await _medicines.ExistsAsync(item.MedicineId)) throw new ArgumentException("One or more selected medicines are invalid.");
        var prescription=new Prescription{AppointmentId=dto.AppointmentId,PatientId=dto.PatientId,DoctorId=dto.DoctorId,PrescriptionDate=dto.PrescriptionDate,Notes=dto.Notes,CreatedOn=DateTime.UtcNow}; await _prescriptions.AddAsync(prescription); await _prescriptions.SaveChangesAsync(); foreach(var line in dto.Items) await _items.AddAsync(new PrescriptionItem{PrescriptionId=prescription.Id,MedicineId=line.MedicineId,Dosage=line.Dosage,Frequency=line.Frequency,DurationInDays=line.DurationInDays,Instructions=line.Instructions}); await _items.SaveChangesAsync(); return prescription.Id;
    }
}

using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<Appointment?> GetWithReferencesAsync(int id);
    Task<IEnumerable<Appointment>> GetAllWithReferencesAsync();
    Task<Appointment?> GetByAppointmentNumberAsync(string appointmentNumber);
    Task<bool> HasSchedulingConflictAsync(int doctorId, DateTime date, TimeSpan time, int? excludeAppointmentId = null);
}

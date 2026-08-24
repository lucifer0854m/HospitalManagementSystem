using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context) { }
    public Task<Appointment?> GetWithReferencesAsync(int id) => _context.Appointments.Include(x => x.Patient).Include(x => x.Doctor).FirstOrDefaultAsync(x => x.Id == id);
    public async Task<IEnumerable<Appointment>> GetAllWithReferencesAsync() => await _context.Appointments.Include(x => x.Patient).Include(x => x.Doctor).OrderByDescending(x => x.AppointmentDate).ThenBy(x => x.AppointmentTime).ToListAsync();
    public Task<Appointment?> GetByAppointmentNumberAsync(string appointmentNumber) => _context.Appointments.FirstOrDefaultAsync(x => x.AppointmentNumber == appointmentNumber);
    public Task<bool> HasSchedulingConflictAsync(int doctorId, DateTime date, TimeSpan time, int? excludeAppointmentId = null) => _context.Appointments.AnyAsync(x => x.DoctorId == doctorId && x.AppointmentDate.Date == date.Date && x.AppointmentTime == time && x.Status != AppointmentStatus.Cancelled && (!excludeAppointmentId.HasValue || x.Id != excludeAppointmentId));
}

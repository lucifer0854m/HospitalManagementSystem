using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using Moq;

namespace HospitalManagement.Tests;

public class AppointmentServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenDoctorAlreadyBooked_RejectsAppointment()
    {
        var (service, appointments) = CreateService(conflict: true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(CreateDto()));
        appointments.Verify(x => x.AddAsync(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithPastDate_RejectsBeforeRepositoryWrites()
    {
        var (service, appointments) = CreateService();

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(CreateDto(DateTime.UtcNow.AddDays(-1))));
        appointments.Verify(x => x.AddAsync(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithValidAppointment_TrimsNumberAndSaves()
    {
        var (service, appointments) = CreateService();
        Appointment? saved = null;
        appointments.Setup(x => x.AddAsync(It.IsAny<Appointment>())).Callback<Appointment>(x => saved = x).Returns(Task.CompletedTask);
        appointments.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        await service.CreateAsync(CreateDto());

        Assert.NotNull(saved);
        Assert.Equal("A-001", saved.AppointmentNumber);
        appointments.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    private static (AppointmentService Service, Mock<IAppointmentRepository> Appointments) CreateService(bool conflict = false)
    {
        var appointments = new Mock<IAppointmentRepository>();
        var patients = new Mock<IGenericRepository<Patient>>();
        var doctors = new Mock<IGenericRepository<Doctor>>();
        patients.Setup(x => x.ExistsAsync(1)).ReturnsAsync(true);
        doctors.Setup(x => x.ExistsAsync(2)).ReturnsAsync(true);
        appointments.Setup(x => x.HasSchedulingConflictAsync(2, It.IsAny<DateTime>(), It.IsAny<TimeSpan>(), null)).ReturnsAsync(conflict);
        appointments.Setup(x => x.GetByAppointmentNumberAsync("A-001")).ReturnsAsync((Appointment?)null);
        return (new AppointmentService(appointments.Object, patients.Object, doctors.Object, TestHelpers.Mapper), appointments);
    }

    private static CreateAppointmentDto CreateDto(DateTime? date = null) => new()
    {
        AppointmentNumber = " A-001 ", PatientId = 1, DoctorId = 2, AppointmentDate = date ?? DateTime.UtcNow.AddDays(1), AppointmentTime = new TimeSpan(10, 0, 0), Reason = "Consultation"
    };
}

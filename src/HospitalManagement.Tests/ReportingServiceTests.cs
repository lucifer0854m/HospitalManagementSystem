using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces;
using Moq;

namespace HospitalManagement.Tests;

public class ReportingServiceTests
{
    [Fact]
    public async Task GetDashboardAsync_ReturnsCurrentOperationalTotals()
    {
        var patients = new Mock<IGenericRepository<Patient>>();
        var doctors = new Mock<IGenericRepository<Doctor>>();
        var appointments = new Mock<IGenericRepository<Appointment>>();
        var requests = new Mock<IGenericRepository<LabRequest>>();
        var bills = new Mock<IGenericRepository<Bill>>();
        var payments = new Mock<IGenericRepository<Payment>>();
        patients.Setup(x => x.GetAllAsync()).ReturnsAsync([new Patient(), new Patient()]);
        doctors.Setup(x => x.GetAllAsync()).ReturnsAsync([new Doctor()]);
        appointments.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Appointment, bool>>>())).ReturnsAsync([new Appointment(), new Appointment(), new Appointment()]);
        requests.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<LabRequest, bool>>>())).ReturnsAsync([new LabRequest { Status = LabRequestStatus.Ordered }]);
        payments.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Payment, bool>>>())).ReturnsAsync([new Payment { Amount = 1250m }]);
        var service = new ReportingService(patients.Object, doctors.Object, appointments.Object, requests.Object, bills.Object, payments.Object);

        var dashboard = await service.GetDashboardAsync();

        Assert.Equal(2, dashboard.TotalPatients);
        Assert.Equal(1, dashboard.TotalDoctors);
        Assert.Equal(3, dashboard.TodayAppointments);
        Assert.Equal(1, dashboard.PendingLabRequests);
        Assert.Equal(1250m, dashboard.TodayRevenue);
    }

    [Fact]
    public async Task GetReportAsync_WithEndBeforeStart_RejectsRequest()
    {
        var service = new ReportingService(
            Mock.Of<IGenericRepository<Patient>>(), Mock.Of<IGenericRepository<Doctor>>(), Mock.Of<IGenericRepository<Appointment>>(),
            Mock.Of<IGenericRepository<LabRequest>>(), Mock.Of<IGenericRepository<Bill>>(), Mock.Of<IGenericRepository<Payment>>());

        await Assert.ThrowsAsync<ArgumentException>(() => service.GetReportAsync(new DateTime(2026, 2, 2), new DateTime(2026, 2, 1)));
    }
}

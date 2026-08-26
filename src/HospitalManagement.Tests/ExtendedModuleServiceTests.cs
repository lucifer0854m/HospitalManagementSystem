using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using Moq;

namespace HospitalManagement.Tests;

public class ExtendedModuleServiceTests
{
    [Fact]
    public async Task CreatePrescriptionAsync_RejectsPatientOrDoctorThatDoesNotMatchAppointment()
    {
        var appointments = new Mock<IGenericRepository<Appointment>>();
        var patients = new Mock<IGenericRepository<Patient>>();
        var doctors = new Mock<IGenericRepository<Doctor>>();
        appointments.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Appointment { Id = 1, PatientId = 2, DoctorId = 3 });
        patients.Setup(x => x.ExistsAsync(9)).ReturnsAsync(true);
        doctors.Setup(x => x.ExistsAsync(3)).ReturnsAsync(true);
        var service = new PrescriptionService(Mock.Of<IGenericRepository<Prescription>>(), Mock.Of<IGenericRepository<PrescriptionItem>>(), appointments.Object, patients.Object, doctors.Object, Mock.Of<IGenericRepository<Medicine>>());

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreatePrescriptionAsync(new CreatePrescriptionDto { AppointmentId = 1, PatientId = 9, DoctorId = 3, Items = [new PrescriptionLineDto { MedicineId = 1, Dosage = "1 tablet", Frequency = "Daily", DurationInDays = 3 }] }));
    }

    [Fact]
    public async Task GetMedicineByIdAsync_ReturnsMedicineDetails()
    {
        var medicines = new Mock<IGenericRepository<Medicine>>();
        medicines.Setup(x => x.GetByIdAsync(4)).ReturnsAsync(new Medicine { Id = 4, MedicineCode = "MED-004", MedicineName = "Paracetamol", Unit = "Tablet", UnitPrice = 5, IsActive = true });
        var service = new PharmacyService(medicines.Object, Mock.Of<IGenericRepository<Inventory>>());

        var result = await service.GetMedicineByIdAsync(4);

        Assert.NotNull(result);
        Assert.Equal("Paracetamol", result.MedicineName);
        Assert.Equal("MED-004", result.MedicineCode);
    }

    [Fact]
    public async Task GetTestByIdAsync_ReturnsTestDetails()
    {
        var tests = new Mock<IGenericRepository<LabTest>>();
        tests.Setup(x => x.GetByIdAsync(5)).ReturnsAsync(new LabTest { Id = 5, TestCode = "LAB-005", TestName = "Blood Count", Price = 250, IsActive = true });
        var service = new LaboratoryService(tests.Object, Mock.Of<IGenericRepository<LabRequest>>(), Mock.Of<IGenericRepository<LabResult>>(), Mock.Of<IGenericRepository<Patient>>(), Mock.Of<IGenericRepository<Appointment>>(), Mock.Of<IGenericRepository<Doctor>>());

        var result = await service.GetTestByIdAsync(5);

        Assert.NotNull(result);
        Assert.Equal("Blood Count", result.TestName);
        Assert.Equal("LAB-005", result.TestCode);
    }

    [Fact]
    public async Task CreateBillAsync_RejectsUnknownPatient()
    {
        var patients = new Mock<IGenericRepository<Patient>>();
        patients.Setup(x => x.ExistsAsync(99)).ReturnsAsync(false);
        var service = new BillingService(Mock.Of<IGenericRepository<Bill>>(), Mock.Of<IGenericRepository<BillItem>>(), Mock.Of<IGenericRepository<Payment>>(), patients.Object, Mock.Of<IGenericRepository<Appointment>>());

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateBillAsync(new CreateBillDto { BillNumber = "B-001", PatientId = 99, Items = [new BillLineDto { ItemName = "Consultation", Quantity = 1, UnitPrice = 500 }] }));
    }

    [Fact]
    public async Task CreateBillAsync_RejectsAppointmentThatAlreadyHasABill()
    {
        var bills = new Mock<IGenericRepository<Bill>>();
        var patients = new Mock<IGenericRepository<Patient>>();
        var appointments = new Mock<IGenericRepository<Appointment>>();
        patients.Setup(x => x.ExistsAsync(2)).ReturnsAsync(true);
        appointments.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Appointment { Id = 1, PatientId = 2 });
        var existingBill = new Bill { BillNumber = "B-EXISTING", AppointmentId = 1 };
        bills.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Bill, bool>>>() ))
            .ReturnsAsync((System.Linq.Expressions.Expression<Func<Bill, bool>> predicate) => predicate.Compile()(existingBill) ? [existingBill] : []);
        var service = new BillingService(bills.Object, Mock.Of<IGenericRepository<BillItem>>(), Mock.Of<IGenericRepository<Payment>>(), patients.Object, appointments.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateBillAsync(new CreateBillDto { BillNumber = "B-NEW", PatientId = 2, AppointmentId = 1, Items = [new BillLineDto { ItemName = "Consultation", Quantity = 1, UnitPrice = 500 }] }));

        Assert.Equal("A bill already exists for this appointment.", exception.Message);
        bills.Verify(x => x.AddAsync(It.IsAny<Bill>()), Times.Never);
    }

    [Fact]
    public async Task SaveMedicineAsync_RejectsDuplicateCode()
    {
        var medicines = new Mock<IGenericRepository<Medicine>>();
        medicines.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Medicine, bool>>>())).ReturnsAsync([new Medicine { Id = 4, MedicineCode = "MED-001" }]);
        var service = new PharmacyService(medicines.Object, Mock.Of<IGenericRepository<Inventory>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SaveMedicineAsync(new SaveMedicineDto { MedicineCode = "MED-001", MedicineName = "Paracetamol" }));
        medicines.Verify(x => x.AddAsync(It.IsAny<Medicine>()), Times.Never);
    }

    [Fact]
    public async Task SaveInventoryAsync_RejectsExpiredStock()
    {
        var medicines = new Mock<IGenericRepository<Medicine>>();
        medicines.Setup(x => x.ExistsAsync(1)).ReturnsAsync(true);
        var service = new PharmacyService(medicines.Object, Mock.Of<IGenericRepository<Inventory>>());

        await Assert.ThrowsAsync<ArgumentException>(() => service.SaveInventoryAsync(new SaveInventoryDto { MedicineId = 1, AvailableQuantity = 10, ReorderLevel = 2, ExpiryDate = DateTime.UtcNow.AddDays(-1) }));
    }

    [Fact]
    public async Task CreateRequestAsync_RejectsDuplicateRequestNumber()
    {
        var tests = new Mock<IGenericRepository<LabTest>>();
        var requests = new Mock<IGenericRepository<LabRequest>>();
        var patients = new Mock<IGenericRepository<Patient>>();
        tests.Setup(x => x.ExistsAsync(1)).ReturnsAsync(true);
        patients.Setup(x => x.ExistsAsync(2)).ReturnsAsync(true);
        requests.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<LabRequest, bool>>>())).ReturnsAsync([new LabRequest { RequestNumber = "LR-001" }]);
        var service = new LaboratoryService(tests.Object, requests.Object, Mock.Of<IGenericRepository<LabResult>>(), patients.Object, Mock.Of<IGenericRepository<Appointment>>(), Mock.Of<IGenericRepository<Doctor>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateRequestAsync(new CreateLabRequestDto { RequestNumber = "LR-001", LabTestId = 1, PatientId = 2 }));
    }

    [Fact]
    public async Task RecordResultAsync_RejectsSecondResult()
    {
        var requests = new Mock<IGenericRepository<LabRequest>>();
        var results = new Mock<IGenericRepository<LabResult>>();
        requests.Setup(x => x.GetByIdAsync(8)).ReturnsAsync(new LabRequest { Id = 8 });
        results.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<LabResult, bool>>>())).ReturnsAsync([new LabResult { LabRequestId = 8 }]);
        var service = new LaboratoryService(Mock.Of<IGenericRepository<LabTest>>(), requests.Object, results.Object, Mock.Of<IGenericRepository<Patient>>(), Mock.Of<IGenericRepository<Appointment>>(), Mock.Of<IGenericRepository<Doctor>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RecordResultAsync(new RecordLabResultDto { LabRequestId = 8, ResultValue = "Normal" }));
        results.Verify(x => x.AddAsync(It.IsAny<LabResult>()), Times.Never);
    }
}

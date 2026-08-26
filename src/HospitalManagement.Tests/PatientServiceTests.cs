using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using Moq;

namespace HospitalManagement.Tests;

public class PatientServiceTests
{
    [Fact]
    public async Task CreateAsync_WithDuplicatePatientCode_ThrowsAndDoesNotSave()
    {
        var repository = new Mock<IPatientRepository>();
        repository.Setup(x => x.GetByPatientCodeAsync("P-001")).ReturnsAsync(new Patient { Id = 4, PatientCode = "P-001" });
        var service = new PatientService(repository.Object, TestHelpers.Mapper);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(CreateDto(" P-001 ")));
        repository.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithFutureDateOfBirth_ThrowsArgumentException()
    {
        var repository = new Mock<IPatientRepository>();
        var service = new PatientService(repository.Object, TestHelpers.Mapper);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(CreateDto("P-002", DateTime.UtcNow.AddDays(1))));
        repository.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithValidPatient_TrimsFieldsAndSaves()
    {
        var repository = new Mock<IPatientRepository>();
        repository.Setup(x => x.GetByPatientCodeAsync("P-003")).ReturnsAsync((Patient?)null);
        Patient? saved = null;
        repository.Setup(x => x.AddAsync(It.IsAny<Patient>())).Callback<Patient>(x => saved = x).Returns(Task.CompletedTask);
        repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        var service = new PatientService(repository.Object, TestHelpers.Mapper);

        await service.CreateAsync(CreateDto(" P-003 "));

        Assert.NotNull(saved);
        Assert.Equal("P-003", saved.PatientCode);
        Assert.Equal("Jane", saved.FirstName);
        repository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithoutAddress_ThrowsAndDoesNotSave()
    {
        var repository = new Mock<IPatientRepository>();
        repository.Setup(x => x.GetByPatientCodeAsync("P-004")).ReturnsAsync((Patient?)null);
        var service = new PatientService(repository.Object, TestHelpers.Mapper);
        var patient = CreateDto("P-004");
        patient.Address = null;

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(patient));

        repository.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    private static CreatePatientDto CreateDto(string code, DateTime? dob = null) => new()
    {
        PatientCode = code,
        FirstName = " Jane ",
        LastName = " Doe ",
        DateOfBirth = dob ?? new DateTime(1990, 1, 1),
        MobileNumber = "9999999999",
        Address = "1 Main Street",
        City = "Bhopal",
        State = "Madhya Pradesh",
        Country = "India",
        Pincode = "462001",
        EmergencyContactName = "John Doe",
        EmergencyContactNumber = "9888888888"
    };
}

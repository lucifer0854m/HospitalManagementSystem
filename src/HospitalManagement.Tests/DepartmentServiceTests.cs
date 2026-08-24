using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using Moq;

namespace HospitalManagement.Tests;

public class DepartmentServiceTests
{
    [Fact]
    public async Task CreateAsync_WithDuplicateCode_ThrowsAndDoesNotSave()
    {
        var departments = new Mock<IDepartmentRepository>();
        departments.Setup(x => x.GetByDepartmentCodeAsync("CARD")).ReturnsAsync(new Department { Id = 1, DepartmentCode = "CARD" });
        var service = new DepartmentService(departments.Object, Mock.Of<IGenericRepository<Doctor>>(), TestHelpers.Mapper);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new CreateDepartmentDto { DepartmentCode = " CARD ", Name = "Cardiology" }));
        departments.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithAssignedDoctors_RejectsDelete()
    {
        var departments = new Mock<IDepartmentRepository>();
        var doctors = new Mock<IGenericRepository<Doctor>>();
        departments.Setup(x => x.GetByIdAsync(3)).ReturnsAsync(new Department { Id = 3 });
        doctors.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Doctor, bool>>>())).ReturnsAsync([new Doctor { Id = 9, DepartmentId = 3 }]);
        var service = new DepartmentService(departments.Object, doctors.Object, TestHelpers.Mapper);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(3));
        departments.Verify(x => x.Delete(It.IsAny<Department>()), Times.Never);
    }
}

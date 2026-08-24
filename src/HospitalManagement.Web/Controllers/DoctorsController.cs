using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagement.Web.Controllers;

public class DoctorsController : Controller
{
    private readonly IDoctorService _doctorService;
    private readonly IGenericRepository<Department> _departmentRepository;

    public DoctorsController(IDoctorService doctorService, IGenericRepository<Department> departmentRepository)
    {
        _doctorService = doctorService;
        _departmentRepository = departmentRepository;
    }

    public async Task<IActionResult> Index() => View(await _doctorService.GetAllAsync());

    public async Task<IActionResult> Create()
    {
        await PopulateDepartmentsAsync();
        return View(new CreateDoctorDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDoctorDto doctor)
    {
        if (!ModelState.IsValid) { await PopulateDepartmentsAsync(); return View(doctor); }
        try
        {
            await _doctorService.CreateAsync(doctor);
            TempData["SuccessMessage"] = "Doctor created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException exception) { ModelState.AddModelError(string.Empty, exception.Message); }
        catch (InvalidOperationException exception) { ModelState.AddModelError(nameof(doctor.DoctorCode), exception.Message); }
        await PopulateDepartmentsAsync();
        return View(doctor);
    }

    public async Task<IActionResult> Details(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        return doctor is null ? NotFound() : View(doctor);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        if (doctor is null) return NotFound();
        await PopulateDepartmentsAsync();
        return View(new UpdateDoctorDto
        {
            Id = doctor.Id, DoctorCode = doctor.DoctorCode, FirstName = doctor.FirstName, LastName = doctor.LastName,
            Gender = Enum.Parse<HospitalManagement.Domain.Enums.Gender>(doctor.Gender), DateOfBirth = doctor.DateOfBirth,
            Qualification = doctor.Qualification, Specialization = doctor.Specialization, ExperienceInYears = doctor.ExperienceInYears,
            ConsultationFee = doctor.ConsultationFee, MobileNumber = doctor.MobileNumber, Email = doctor.Email,
            Address = doctor.Address, JoiningDate = doctor.JoiningDate, IsAvailable = doctor.IsAvailable, DepartmentId = doctor.DepartmentId
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateDoctorDto doctor)
    {
        if (id != doctor.Id) return NotFound();
        if (!ModelState.IsValid) { await PopulateDepartmentsAsync(); return View(doctor); }
        try
        {
            await _doctorService.UpdateAsync(doctor);
            TempData["SuccessMessage"] = "Doctor updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException exception) { ModelState.AddModelError(string.Empty, exception.Message); }
        catch (InvalidOperationException exception) { ModelState.AddModelError(nameof(doctor.DoctorCode), exception.Message); }
        await PopulateDepartmentsAsync();
        return View(doctor);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        return doctor is null ? NotFound() : View(doctor);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try { await _doctorService.DeleteAsync(id); }
        catch (KeyNotFoundException) { return NotFound(); }
        TempData["SuccessMessage"] = "Doctor deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDepartmentsAsync() =>
        ViewBag.Departments = new SelectList(await _departmentRepository.GetAllAsync(), nameof(Department.Id), nameof(Department.Name));
}

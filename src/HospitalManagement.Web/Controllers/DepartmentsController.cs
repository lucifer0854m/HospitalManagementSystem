using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly IDepartmentService _departmentService;
    public DepartmentsController(IDepartmentService departmentService) => _departmentService = departmentService;
    public async Task<IActionResult> Index() => View(await _departmentService.GetAllAsync());
    public IActionResult Create() => View(new CreateDepartmentDto());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDepartmentDto department)
    {
        if (!ModelState.IsValid) return View(department);
        try { await _departmentService.CreateAsync(department); TempData["SuccessMessage"] = "Department created successfully."; return RedirectToAction(nameof(Index)); }
        catch (ArgumentException exception) { ModelState.AddModelError(string.Empty, exception.Message); }
        catch (InvalidOperationException exception) { ModelState.AddModelError(nameof(department.DepartmentCode), exception.Message); }
        return View(department);
    }
    public async Task<IActionResult> Details(int id) { var department = await _departmentService.GetByIdAsync(id); return department is null ? NotFound() : View(department); }
    public async Task<IActionResult> Edit(int id)
    {
        var department = await _departmentService.GetByIdAsync(id); if (department is null) return NotFound();
        return View(new UpdateDepartmentDto { Id = department.Id, DepartmentCode = department.DepartmentCode, Name = department.Name, Description = department.Description, Location = department.Location, IsActive = department.IsActive });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateDepartmentDto department)
    {
        if (id != department.Id) return NotFound(); if (!ModelState.IsValid) return View(department);
        try { await _departmentService.UpdateAsync(department); TempData["SuccessMessage"] = "Department updated successfully."; return RedirectToAction(nameof(Index)); }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException exception) { ModelState.AddModelError(string.Empty, exception.Message); }
        catch (InvalidOperationException exception) { ModelState.AddModelError(nameof(department.DepartmentCode), exception.Message); }
        return View(department);
    }
    public async Task<IActionResult> Delete(int id) { var department = await _departmentService.GetByIdAsync(id); return department is null ? NotFound() : View(department); }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try { await _departmentService.DeleteAsync(id); }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException exception) { TempData["ErrorMessage"] = exception.Message; return RedirectToAction(nameof(Index)); }
        TempData["SuccessMessage"] = "Department deleted successfully."; return RedirectToAction(nameof(Index));
    }
}

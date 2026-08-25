using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin + "," + HospitalRoles.Doctor + "," + HospitalRoles.LabTechnician)]
public class LaboratoryController : Controller
{
    private readonly ILaboratoryService _laboratory;
    private readonly IPatientService _patients;
    private readonly IDoctorService _doctors;
    private readonly IAppointmentService _appointments;
    public LaboratoryController(ILaboratoryService laboratory, IPatientService patients, IDoctorService doctors, IAppointmentService appointments) => (_laboratory, _patients, _doctors, _appointments) = (laboratory, patients, doctors, appointments);
    public async Task<IActionResult> Index() => View(new LaboratoryIndexViewModel { Tests = await _laboratory.GetTestsAsync(), Requests = await _laboratory.GetRequestsAsync() });
    public IActionResult CreateTest() => View(new SaveLabTestDto());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> CreateTest(SaveLabTestDto model) { if (!ModelState.IsValid) return View(model); try { await _laboratory.SaveTestAsync(model); TempData["SuccessMessage"] = "Lab test saved."; return RedirectToAction(nameof(Index)); } catch (InvalidOperationException e) { ModelState.AddModelError(string.Empty, e.Message); return View(model); } }
    public async Task<IActionResult> CreateRequest() { await PopulateRequestChoices(); return View(new CreateLabRequestDto()); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> CreateRequest(CreateLabRequestDto model) { if (!ModelState.IsValid) { await PopulateRequestChoices(); return View(model); } try { await _laboratory.CreateRequestAsync(model); TempData["SuccessMessage"] = "Lab request created."; return RedirectToAction(nameof(Index)); } catch (Exception e) when (e is ArgumentException or InvalidOperationException) { ModelState.AddModelError(string.Empty, e.Message); await PopulateRequestChoices(); return View(model); } }
    public async Task<IActionResult> RecordResult() { ViewBag.Requests = await _laboratory.GetRequestsAsync(); return View(new RecordLabResultDto()); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> RecordResult(RecordLabResultDto model) { if (!ModelState.IsValid) { ViewBag.Requests = await _laboratory.GetRequestsAsync(); return View(model); } try { await _laboratory.RecordResultAsync(model); TempData["SuccessMessage"] = "Result recorded."; return RedirectToAction(nameof(Index)); } catch (Exception e) when (e is KeyNotFoundException or InvalidOperationException) { ModelState.AddModelError(string.Empty, e.Message); ViewBag.Requests = await _laboratory.GetRequestsAsync(); return View(model); } }
    private async Task PopulateRequestChoices() { ViewBag.Tests = await _laboratory.GetTestsAsync(); ViewBag.Patients = await _patients.GetAllAsync(); ViewBag.Doctors = await _doctors.GetAllAsync(); ViewBag.Appointments = await _appointments.GetAllAsync(); }
}

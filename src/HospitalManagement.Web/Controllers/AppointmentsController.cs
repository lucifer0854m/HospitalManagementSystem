using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin + "," + HospitalRoles.Doctor + "," + HospitalRoles.Receptionist)]
public class AppointmentsController : Controller
{
    private readonly IAppointmentService _appointmentService;
    private readonly IPatientService _patientService;
    private readonly IDoctorService _doctorService;

    public AppointmentsController(IAppointmentService appointmentService, IPatientService patientService, IDoctorService doctorService) => (_appointmentService, _patientService, _doctorService) = (appointmentService, patientService, doctorService);
    public async Task<IActionResult> Index() => View(await _appointmentService.GetAllAsync());
    public async Task<IActionResult> Create() { await PopulateSelectionsAsync(); return View(new CreateAppointmentDto()); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAppointmentDto appointment)
    {
        if (!ModelState.IsValid) { await PopulateSelectionsAsync(); return View(appointment); }
        try { await _appointmentService.CreateAsync(appointment); TempData["SuccessMessage"] = "Appointment scheduled successfully."; return RedirectToAction(nameof(Index)); }
        catch (ArgumentException exception) { ModelState.AddModelError(string.Empty, exception.Message); }
        catch (InvalidOperationException exception) { ModelState.AddModelError(string.Empty, exception.Message); }
        await PopulateSelectionsAsync(); return View(appointment);
    }

    public async Task<IActionResult> Details(int id) { var appointment = await _appointmentService.GetByIdAsync(id); return appointment is null ? NotFound() : View(appointment); }
    public async Task<IActionResult> Edit(int id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id); if (appointment is null) return NotFound();
        await PopulateSelectionsAsync();
        return View(new UpdateAppointmentDto { Id = appointment.Id, AppointmentNumber = appointment.AppointmentNumber, PatientId = appointment.PatientId, DoctorId = appointment.DoctorId, AppointmentDate = appointment.AppointmentDate, AppointmentTime = appointment.AppointmentTime, AppointmentType = Enum.Parse<AppointmentType>(appointment.AppointmentType), Status = Enum.Parse<AppointmentStatus>(appointment.Status), Reason = appointment.Reason, Symptoms = appointment.Symptoms, Notes = appointment.Notes, IsFollowUp = appointment.IsFollowUp, FollowUpDate = appointment.FollowUpDate });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateAppointmentDto appointment)
    {
        if (id != appointment.Id) return NotFound();
        if (!ModelState.IsValid) { await PopulateSelectionsAsync(); return View(appointment); }
        try { await _appointmentService.UpdateAsync(appointment); TempData["SuccessMessage"] = "Appointment updated successfully."; return RedirectToAction(nameof(Index)); }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException exception) { ModelState.AddModelError(string.Empty, exception.Message); }
        catch (InvalidOperationException exception) { ModelState.AddModelError(string.Empty, exception.Message); }
        await PopulateSelectionsAsync(); return View(appointment);
    }

    public async Task<IActionResult> Delete(int id) { var appointment = await _appointmentService.GetByIdAsync(id); return appointment is null ? NotFound() : View(appointment); }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) { try { await _appointmentService.DeleteAsync(id); } catch (KeyNotFoundException) { return NotFound(); } TempData["SuccessMessage"] = "Appointment deleted successfully."; return RedirectToAction(nameof(Index)); }
    private async Task PopulateSelectionsAsync()
    {
        var patients = await _patientService.GetAllAsync(); var doctors = await _doctorService.GetAllAsync();
        ViewBag.Patients = new SelectList(patients.Select(x => new { x.Id, Name = $"{x.PatientCode} — {x.FullName}" }), "Id", "Name");
        ViewBag.Doctors = new SelectList(doctors.Select(x => new { x.Id, Name = $"{x.DoctorCode} — Dr. {x.FullName}" }), "Id", "Name");
    }
}

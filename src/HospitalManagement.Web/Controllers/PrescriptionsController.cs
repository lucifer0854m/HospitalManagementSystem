using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin + "," + HospitalRoles.Doctor)]
public class PrescriptionsController : Controller
{
    private readonly IPrescriptionService _prescriptions;
    private readonly IAppointmentService _appointments;
    private readonly IPatientService _patients;
    private readonly IDoctorService _doctors;
    private readonly IPharmacyService _pharmacy;

    public PrescriptionsController(IPrescriptionService prescriptions, IAppointmentService appointments, IPatientService patients, IDoctorService doctors, IPharmacyService pharmacy) =>
        (_prescriptions, _appointments, _patients, _doctors, _pharmacy) = (prescriptions, appointments, patients, doctors, pharmacy);

    public async Task<IActionResult> Index() => View(await _prescriptions.GetAllAsync());

    public async Task<IActionResult> Create()
    {
        await PopulateSelectionsAsync();
        return View(new CreatePrescriptionDto { Items = [new PrescriptionLineDto()] });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePrescriptionDto model)
    {
        if (model.Items.Count == 0) ModelState.AddModelError(nameof(model.Items), "Add at least one medicine.");
        if (!ModelState.IsValid)
        {
            await PopulateSelectionsAsync();
            return View(model);
        }

        try
        {
            var id = await _prescriptions.CreatePrescriptionAsync(model);
            TempData["SuccessMessage"] = "Prescription created successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
        }

        await PopulateSelectionsAsync();
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var prescription = await _prescriptions.GetByIdAsync(id);
        return prescription is null ? NotFound() : View(prescription);
    }

    private async Task PopulateSelectionsAsync()
    {
        ViewBag.Appointments = await _appointments.GetAllAsync();
        ViewBag.Patients = await _patients.GetAllAsync();
        ViewBag.Doctors = await _doctors.GetAllAsync();
        ViewBag.Medicines = (await _pharmacy.GetMedicinesAsync()).Where(x => x.IsActive);
    }
}

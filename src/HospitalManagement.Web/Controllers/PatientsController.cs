using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin + "," + HospitalRoles.Doctor + "," + HospitalRoles.Receptionist)]
public class PatientsController : Controller
{
    // Patient views predate the pluralized controller name and live in Views/Patient.
    // Use their explicit path so MVC does not search the non-existent Views/Patients folder.
    private const string PatientViewPath = "~/Views/Patient/";

    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService) => _patientService = patientService;

    public async Task<IActionResult> Index() => View(PatientView("Index"), await _patientService.GetAllAsync());

    public IActionResult Create() => View(PatientView("Create"), new CreatePatientDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePatientDto patient)
    {
        if (!ModelState.IsValid) return View(PatientView("Create"), patient);
        try
        {
            await _patientService.CreateAsync(patient);
            TempData["SuccessMessage"] = "Patient created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(PatientView("Create"), patient);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(patient.PatientCode), exception.Message);
            return View(PatientView("Create"), patient);
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        return patient is null ? NotFound() : View(PatientView("Details"), patient);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        if (patient is null) return NotFound();

        return View(PatientView("Edit"), new UpdatePatientDto
        {
            Id = patient.Id, PatientCode = patient.PatientCode,
            FirstName = patient.FirstName, LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Gender = Enum.Parse<Gender>(patient.Gender),
            BloodGroup = Enum.Parse<BloodGroup>(patient.BloodGroup),
            MobileNumber = patient.MobileNumber, Email = patient.Email,
            Address = patient.Address, City = patient.City, State = patient.State,
            Country = patient.Country, Pincode = patient.Pincode,
            EmergencyContactName = patient.EmergencyContactName,
            EmergencyContactNumber = patient.EmergencyContactNumber,
            EmergencyContactRelation = patient.EmergencyContactRelation,
            Height = patient.Height, Weight = patient.Weight, Allergies = patient.Allergies,
            MedicalHistory = patient.MedicalHistory,
            Status = Enum.Parse<PatientStatus>(patient.Status)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdatePatientDto patient)
    {
        if (id != patient.Id) return NotFound();
        if (!ModelState.IsValid) return View(PatientView("Edit"), patient);
        try
        {
            await _patientService.UpdateAsync(patient);
            TempData["SuccessMessage"] = "Patient updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(PatientView("Edit"), patient);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(patient.PatientCode), exception.Message);
            return View(PatientView("Edit"), patient);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        return patient is null ? NotFound() : View(PatientView("Delete"), patient);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _patientService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Patient deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    private static string PatientView(string name) => $"{PatientViewPath}{name}.cshtml";
}

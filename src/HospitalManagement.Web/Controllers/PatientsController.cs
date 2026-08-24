using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

public class PatientsController : Controller
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    public async Task<IActionResult> Index()
    {
        var patients = await _patientService.GetAllAsync();

        return View(patients);
    }
}
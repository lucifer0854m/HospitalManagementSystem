using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin + "," + HospitalRoles.Receptionist)]
public class BillingController : Controller
{
    private readonly IBillingService _billing;
    private readonly IPatientService _patients;
    private readonly IAppointmentService _appointments;
    public BillingController(IBillingService billing, IPatientService patients, IAppointmentService appointments) => (_billing, _patients, _appointments) = (billing, patients, appointments);
    public async Task<IActionResult> Index() => View(await BuildIndexAsync());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBillDto model)
    {
        if (!ModelState.IsValid) return View("Index", await BuildIndexAsync(model));
        try { await _billing.CreateBillAsync(model); TempData["SuccessMessage"] = "Bill created successfully."; return RedirectToAction(nameof(Index)); }
        catch (Exception e) when (e is ArgumentException or InvalidOperationException) { ModelState.AddModelError(string.Empty, e.Message); return View("Index", await BuildIndexAsync(model)); }
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RecordPayment(RecordPaymentDto model)
    {
        try { var summary = await _billing.RecordPaymentAsync(model); TempData["SuccessMessage"] = $"Payment recorded. Balance: {summary.Balance:N2}"; }
        catch (Exception e) when (e is ArgumentException or KeyNotFoundException) { TempData["ErrorMessage"] = e.Message; }
        return RedirectToAction(nameof(Index));
    }
    private async Task<BillingIndexViewModel> BuildIndexAsync(CreateBillDto? bill = null) { ViewBag.Patients = await _patients.GetAllAsync(); ViewBag.Appointments = await _appointments.GetAllAsync(); return new BillingIndexViewModel { NewBill = bill ?? new CreateBillDto { Items = [new BillLineDto()] }, Bills = await _billing.GetBillsAsync() }; }
}

using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin + "," + HospitalRoles.Pharmacist)]
public class PharmacyController : Controller
{
    private readonly IPharmacyService _pharmacy;
    public PharmacyController(IPharmacyService pharmacy) => _pharmacy = pharmacy;
    public async Task<IActionResult> Index() => View(new PharmacyIndexViewModel { Medicines = await _pharmacy.GetMedicinesAsync(), Inventory = await _pharmacy.GetInventoryAsync(), LowStock = await _pharmacy.GetLowStockAsync() });
    public IActionResult CreateMedicine() => View(new SaveMedicineDto());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> CreateMedicine(SaveMedicineDto model) { if (!ModelState.IsValid) return View(model); try { await _pharmacy.SaveMedicineAsync(model); TempData["SuccessMessage"] = "Medicine saved."; return RedirectToAction(nameof(Index)); } catch (InvalidOperationException e) { ModelState.AddModelError(string.Empty, e.Message); return View(model); } }
    public async Task<IActionResult> CreateInventory() { ViewBag.Medicines = await _pharmacy.GetMedicinesAsync(); return View(new SaveInventoryDto()); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> CreateInventory(SaveInventoryDto model) { if (!ModelState.IsValid) { ViewBag.Medicines = await _pharmacy.GetMedicinesAsync(); return View(model); } try { await _pharmacy.SaveInventoryAsync(model); TempData["SuccessMessage"] = "Inventory saved."; return RedirectToAction(nameof(Index)); } catch (ArgumentException e) { ViewBag.Medicines = await _pharmacy.GetMedicinesAsync(); ModelState.AddModelError(string.Empty, e.Message); return View(model); } }
}

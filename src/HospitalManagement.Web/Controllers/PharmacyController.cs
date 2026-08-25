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
    public async Task<IActionResult> EditMedicine(int id)
    {
        var medicine = await _pharmacy.GetMedicineByIdAsync(id);
        if (medicine is null) return NotFound();
        ViewData["FormAction"] = nameof(EditMedicine);
        ViewData["Title"] = "Edit medicine";
        return View("CreateMedicine", new SaveMedicineDto { MedicineCode=medicine.MedicineCode, MedicineName=medicine.MedicineName, GenericName=medicine.GenericName, Manufacturer=medicine.Manufacturer, Unit=medicine.Unit, UnitPrice=medicine.UnitPrice, IsActive=medicine.IsActive });
    }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> EditMedicine(int id, SaveMedicineDto model) { if (!ModelState.IsValid) { ViewData["FormAction"] = nameof(EditMedicine); return View("CreateMedicine", model); } try { await _pharmacy.SaveMedicineAsync(model, id); TempData["SuccessMessage"] = "Medicine updated."; return RedirectToAction(nameof(Index)); } catch (Exception e) when (e is InvalidOperationException or KeyNotFoundException) { ModelState.AddModelError(string.Empty, e.Message); ViewData["FormAction"] = nameof(EditMedicine); return View("CreateMedicine", model); } }
    public async Task<IActionResult> CreateInventory() { ViewBag.Medicines = await _pharmacy.GetMedicinesAsync(); return View(new SaveInventoryDto()); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> CreateInventory(SaveInventoryDto model) { if (!ModelState.IsValid) { ViewBag.Medicines = await _pharmacy.GetMedicinesAsync(); return View(model); } try { await _pharmacy.SaveInventoryAsync(model); TempData["SuccessMessage"] = "Inventory saved."; return RedirectToAction(nameof(Index)); } catch (ArgumentException e) { ViewBag.Medicines = await _pharmacy.GetMedicinesAsync(); ModelState.AddModelError(string.Empty, e.Message); return View(model); } }
    public async Task<IActionResult> EditInventory(int id)
    {
        var inventory = await _pharmacy.GetInventoryByIdAsync(id);
        if (inventory is null) return NotFound();
        ViewBag.Medicines = await _pharmacy.GetMedicinesAsync(); ViewData["FormAction"] = nameof(EditInventory); ViewData["Title"] = "Edit inventory";
        return View("CreateInventory", new SaveInventoryDto { MedicineId=inventory.MedicineId, AvailableQuantity=inventory.AvailableQuantity, ReorderLevel=inventory.ReorderLevel, ExpiryDate=inventory.ExpiryDate, BatchNumber=inventory.BatchNumber, SupplierName=inventory.SupplierName });
    }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> EditInventory(int id, SaveInventoryDto model) { if (!ModelState.IsValid) { ViewBag.Medicines = await _pharmacy.GetMedicinesAsync(); ViewData["FormAction"] = nameof(EditInventory); return View("CreateInventory", model); } try { await _pharmacy.SaveInventoryAsync(model, id); TempData["SuccessMessage"] = "Inventory updated."; return RedirectToAction(nameof(Index)); } catch (Exception e) when (e is ArgumentException or KeyNotFoundException) { ViewBag.Medicines = await _pharmacy.GetMedicinesAsync(); ViewData["FormAction"] = nameof(EditInventory); ModelState.AddModelError(string.Empty, e.Message); return View("CreateInventory", model); } }
}

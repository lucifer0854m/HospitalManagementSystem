using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin + "," + HospitalRoles.Pharmacist)]
[ApiController]
[Route("api/pharmacy")]
public class PharmacyController : ControllerBase
{
    private readonly IPharmacyService _pharmacy;
    public PharmacyController(IPharmacyService pharmacy) => _pharmacy = pharmacy;
    [HttpGet("medicines")] public async Task<IActionResult> Medicines() => Ok(await _pharmacy.GetMedicinesAsync());
    [HttpPost("medicines")] public async Task<IActionResult> SaveMedicine(SaveMedicineDto dto) { try { var id=await _pharmacy.SaveMedicineAsync(dto); return CreatedAtAction(nameof(Medicines),new{id},new{id}); } catch(InvalidOperationException e){return Conflict(new{e.Message});} }
    [HttpPut("medicines/{id:int}")] public async Task<IActionResult> UpdateMedicine(int id, SaveMedicineDto dto) { try { await _pharmacy.SaveMedicineAsync(dto,id); return NoContent(); } catch(KeyNotFoundException){return NotFound();} catch(InvalidOperationException e){return Conflict(new{e.Message});} }
    [HttpGet("inventory")] public async Task<IActionResult> Inventory() => Ok(await _pharmacy.GetInventoryAsync());
    [HttpGet("inventory/low-stock")] public async Task<IActionResult> LowStock() => Ok(await _pharmacy.GetLowStockAsync());
    [HttpPost("inventory")] public async Task<IActionResult> SaveInventory(SaveInventoryDto dto) { try { var id=await _pharmacy.SaveInventoryAsync(dto); return Ok(new{id}); } catch(ArgumentException e){return BadRequest(new{e.Message});} }
    [HttpPut("inventory/{id:int}")] public async Task<IActionResult> UpdateInventory(int id, SaveInventoryDto dto) { try { await _pharmacy.SaveInventoryAsync(dto,id); return NoContent(); } catch(KeyNotFoundException){return NotFound();} catch(ArgumentException e){return BadRequest(new{e.Message});} }
}

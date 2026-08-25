using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;
public class PharmacyService : IPharmacyService
{
    private readonly IGenericRepository<Medicine> _medicines; private readonly IGenericRepository<Inventory> _inventory;
    public PharmacyService(IGenericRepository<Medicine> medicines, IGenericRepository<Inventory> inventory) => (_medicines, _inventory) = (medicines, inventory);
    public async Task<IEnumerable<MedicineDto>> GetMedicinesAsync() => (await _medicines.GetAllAsync()).OrderBy(x => x.MedicineName).Select(x => new MedicineDto { Id=x.Id, MedicineCode=x.MedicineCode, MedicineName=x.MedicineName, GenericName=x.GenericName, Manufacturer=x.Manufacturer, Unit=x.Unit, UnitPrice=x.UnitPrice, IsActive=x.IsActive });
    public async Task<MedicineDto?> GetMedicineByIdAsync(int id)
    {
        var medicine = await _medicines.GetByIdAsync(id);
        return medicine is null ? null : new MedicineDto { Id=medicine.Id, MedicineCode=medicine.MedicineCode, MedicineName=medicine.MedicineName, GenericName=medicine.GenericName, Manufacturer=medicine.Manufacturer, Unit=medicine.Unit, UnitPrice=medicine.UnitPrice, IsActive=medicine.IsActive };
    }
    public async Task<int> SaveMedicineAsync(SaveMedicineDto dto, int? id = null)
    {
        var duplicate = (await _medicines.FindAsync(x => x.MedicineCode == dto.MedicineCode.Trim())).FirstOrDefault(); if (duplicate is not null && duplicate.Id != id) throw new InvalidOperationException("A medicine with this code already exists.");
        var medicine = id.HasValue ? await _medicines.GetByIdAsync(id.Value) ?? throw new KeyNotFoundException("Medicine not found.") : new Medicine { CreatedOn=DateTime.UtcNow };
        medicine.MedicineCode=dto.MedicineCode.Trim(); medicine.MedicineName=dto.MedicineName.Trim(); medicine.GenericName=dto.GenericName; medicine.Manufacturer=dto.Manufacturer; medicine.Unit=dto.Unit; medicine.UnitPrice=dto.UnitPrice; medicine.IsActive=dto.IsActive; medicine.ModifiedOn=id.HasValue?DateTime.UtcNow:null;
        if (!id.HasValue) await _medicines.AddAsync(medicine); else _medicines.Update(medicine); await _medicines.SaveChangesAsync(); return medicine.Id;
    }
    public async Task<IEnumerable<InventoryDto>> GetInventoryAsync()
    {
        var medicines=(await _medicines.GetAllAsync()).ToDictionary(x=>x.Id,x=>x.MedicineName); return (await _inventory.GetAllAsync()).OrderBy(x=>x.ExpiryDate).Select(x=>new InventoryDto{Id=x.Id,MedicineId=x.MedicineId,MedicineName=medicines.GetValueOrDefault(x.MedicineId,"Unknown"),AvailableQuantity=x.AvailableQuantity,ReorderLevel=x.ReorderLevel,ExpiryDate=x.ExpiryDate,BatchNumber=x.BatchNumber,SupplierName=x.SupplierName});
    }
    public async Task<InventoryDto?> GetInventoryByIdAsync(int id)
    {
        var inventory = await _inventory.GetByIdAsync(id);
        if (inventory is null) return null;
        var medicine = await _medicines.GetByIdAsync(inventory.MedicineId);
        return new InventoryDto { Id=inventory.Id, MedicineId=inventory.MedicineId, MedicineName=medicine?.MedicineName ?? "Unknown", AvailableQuantity=inventory.AvailableQuantity, ReorderLevel=inventory.ReorderLevel, ExpiryDate=inventory.ExpiryDate, BatchNumber=inventory.BatchNumber, SupplierName=inventory.SupplierName };
    }
    public async Task<int> SaveInventoryAsync(SaveInventoryDto dto, int? id = null)
    {
        if (!await _medicines.ExistsAsync(dto.MedicineId)) throw new ArgumentException("Select a valid medicine."); if (dto.ExpiryDate?.Date < DateTime.UtcNow.Date) throw new ArgumentException("Expiry date cannot be in the past.");
        var inventory=id.HasValue?await _inventory.GetByIdAsync(id.Value)??throw new KeyNotFoundException("Inventory record not found."):new Inventory{CreatedOn=DateTime.UtcNow}; inventory.MedicineId=dto.MedicineId; inventory.AvailableQuantity=dto.AvailableQuantity; inventory.ReorderLevel=dto.ReorderLevel; inventory.ExpiryDate=dto.ExpiryDate; inventory.BatchNumber=dto.BatchNumber; inventory.SupplierName=dto.SupplierName; inventory.ModifiedOn=id.HasValue?DateTime.UtcNow:null;
        if(!id.HasValue) await _inventory.AddAsync(inventory); else _inventory.Update(inventory); await _inventory.SaveChangesAsync(); return inventory.Id;
    }
    public async Task<IEnumerable<InventoryDto>> GetLowStockAsync() => (await GetInventoryAsync()).Where(x=>x.AvailableQuantity<=x.ReorderLevel);
}

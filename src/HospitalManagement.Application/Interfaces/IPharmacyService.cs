using HospitalManagement.Application.DTOs;
namespace HospitalManagement.Application.Interfaces;
public interface IPharmacyService
{
    Task<IEnumerable<MedicineDto>> GetMedicinesAsync();
    Task<MedicineDto?> GetMedicineByIdAsync(int id);
    Task<int> SaveMedicineAsync(SaveMedicineDto dto, int? id = null);
    Task<IEnumerable<InventoryDto>> GetInventoryAsync();
    Task<InventoryDto?> GetInventoryByIdAsync(int id);
    Task<int> SaveInventoryAsync(SaveInventoryDto dto, int? id = null);
    Task<IEnumerable<InventoryDto>> GetLowStockAsync();
}

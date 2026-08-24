using HospitalManagement.Application.DTOs;
namespace HospitalManagement.Application.Interfaces;
public interface IPharmacyService
{
    Task<IEnumerable<MedicineDto>> GetMedicinesAsync(); Task<int> SaveMedicineAsync(SaveMedicineDto dto, int? id = null); Task<IEnumerable<InventoryDto>> GetInventoryAsync(); Task<int> SaveInventoryAsync(SaveInventoryDto dto, int? id = null); Task<IEnumerable<InventoryDto>> GetLowStockAsync();
}

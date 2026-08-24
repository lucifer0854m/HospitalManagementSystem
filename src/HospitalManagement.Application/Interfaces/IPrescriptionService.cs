using HospitalManagement.Application.DTOs;
namespace HospitalManagement.Application.Interfaces;
public interface IPrescriptionService { Task<int> CreatePrescriptionAsync(CreatePrescriptionDto dto); }

using HospitalManagement.Application.DTOs;
namespace HospitalManagement.Application.Interfaces;
public interface ILaboratoryService { Task<IEnumerable<LabTestDto>> GetTestsAsync(); Task<int> SaveTestAsync(SaveLabTestDto dto,int? id=null); Task<int> CreateRequestAsync(CreateLabRequestDto dto); Task<int> RecordResultAsync(RecordLabResultDto dto); }

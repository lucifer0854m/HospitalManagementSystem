using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace HospitalManagement.Web.Controllers;
[ApiController, Route("api/laboratory")]
public class LaboratoryController : ControllerBase
{
    private readonly ILaboratoryService _laboratory; public LaboratoryController(ILaboratoryService laboratory)=>_laboratory=laboratory;
    [HttpGet("tests")] public async Task<IActionResult> Tests()=>Ok(await _laboratory.GetTestsAsync());
    [HttpPost("tests")] public async Task<IActionResult> SaveTest(SaveLabTestDto dto){try{return Ok(new{id=await _laboratory.SaveTestAsync(dto)});}catch(InvalidOperationException e){return Conflict(new{e.Message});}}
    [HttpPost("requests")] public async Task<IActionResult> CreateRequest(CreateLabRequestDto dto){try{return Ok(new{id=await _laboratory.CreateRequestAsync(dto)});}catch(ArgumentException e){return BadRequest(new{e.Message});}catch(InvalidOperationException e){return Conflict(new{e.Message});}}
    [HttpPost("results")] public async Task<IActionResult> RecordResult(RecordLabResultDto dto){try{return Ok(new{id=await _laboratory.RecordResultAsync(dto)});}catch(KeyNotFoundException){return NotFound();}catch(InvalidOperationException e){return Conflict(new{e.Message});}}
}

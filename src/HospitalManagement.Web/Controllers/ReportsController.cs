using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace HospitalManagement.Web.Controllers;
[Authorize(Roles = HospitalRoles.Admin)]
[ApiController, Route("api/reports")]
public class ReportsApiController : ControllerBase
{
    private readonly IReportingService _reports; public ReportsApiController(IReportingService reports)=>_reports=reports;
    [HttpGet("dashboard")] public async Task<IActionResult> Dashboard()=>Ok(await _reports.GetDashboardAsync());
    [HttpGet("summary")] public async Task<IActionResult> Summary(DateTime from,DateTime to){try{return Ok(await _reports.GetReportAsync(from,to));}catch(ArgumentException e){return BadRequest(new{e.Message});}}
}

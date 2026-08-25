using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin + "," + HospitalRoles.Doctor)]
[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsApiController : ControllerBase
{
    private readonly IPrescriptionService _prescriptions;

    public PrescriptionsApiController(IPrescriptionService prescriptions) => _prescriptions = prescriptions;

    [HttpPost]
    public async Task<IActionResult> Create(CreatePrescriptionDto dto)
    {
        try
        {
            var id = await _prescriptions.CreatePrescriptionAsync(dto);
            return Ok(new { id });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { exception.Message });
        }
    }
}

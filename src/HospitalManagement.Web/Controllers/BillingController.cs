using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin + "," + HospitalRoles.Receptionist)]
[ApiController]
[Route("api/billing")]
public class BillingController : ControllerBase
{
    private readonly IBillingService _billing;
    public BillingController(IBillingService billing) => _billing = billing;
    [HttpPost("bills")] public async Task<IActionResult> CreateBill(CreateBillDto dto) { try { var id=await _billing.CreateBillAsync(dto); return Ok(new{id}); } catch(ArgumentException e){return BadRequest(new{e.Message});} catch(InvalidOperationException e){return Conflict(new{e.Message});} }
    [HttpPost("payments")] public async Task<IActionResult> RecordPayment(RecordPaymentDto dto) { try { return Ok(await _billing.RecordPaymentAsync(dto)); } catch(KeyNotFoundException){return NotFound();} catch(ArgumentException e){return BadRequest(new{e.Message});} }
}

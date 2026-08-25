using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin)]
public class ReportsController : Controller
{
    private readonly IReportingService _reports;
    public ReportsController(IReportingService reports) => _reports = reports;

    public async Task<IActionResult> Index() => View(await _reports.GetDashboardAsync());

    [HttpGet]
    public async Task<IActionResult> Summary(DateTime? from, DateTime? to)
    {
        var start = from?.Date ?? DateTime.UtcNow.Date.AddDays(-30);
        var end = to?.Date ?? DateTime.UtcNow.Date;
        if (end < start)
        {
            ModelState.AddModelError(string.Empty, "The end date must be on or after the start date.");
            ViewData["From"] = start;
            ViewData["To"] = end;
            return View();
        }

        ViewData["From"] = start;
        ViewData["To"] = end;
        return View(await _reports.GetReportAsync(start, end));
    }
}

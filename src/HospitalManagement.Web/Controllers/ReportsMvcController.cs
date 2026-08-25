using HospitalManagement.Application.Interfaces;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

    [HttpGet]
    public async Task<IActionResult> ExportSummary(DateTime? from, DateTime? to)
    {
        var start = from?.Date ?? DateTime.UtcNow.Date.AddDays(-30);
        var end = to?.Date ?? DateTime.UtcNow.Date;
        if (end < start) return BadRequest("The end date must be on or after the start date.");

        var report = await _reports.GetReportAsync(start, end);
        var csv = new StringBuilder()
            .AppendLine("Metric,Value")
            .AppendLine($"From,{report.From:yyyy-MM-dd}")
            .AppendLine($"To,{report.To:yyyy-MM-dd}")
            .AppendLine($"Appointments,{report.Appointments}")
            .AppendLine($"Completed appointments,{report.CompletedAppointments}")
            .AppendLine($"Lab requests,{report.LabRequests}")
            .AppendLine($"Billed amount,{report.BilledAmount}")
            .AppendLine($"Payments received,{report.PaidAmount}")
            .AppendLine($"Outstanding,{report.BilledAmount - report.PaidAmount}");
        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"hospital-summary-{start:yyyyMMdd}-{end:yyyyMMdd}.csv");
    }
}

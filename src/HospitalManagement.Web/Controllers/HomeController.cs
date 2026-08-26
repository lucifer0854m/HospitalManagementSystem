using System.Diagnostics;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Web.Models;

namespace HospitalManagement.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IReportingService _reports;

    public HomeController(ILogger<HomeController> logger, IReportingService reports)
    {
        _logger = logger;
        _reports = reports;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _reports.GetDashboardAsync());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

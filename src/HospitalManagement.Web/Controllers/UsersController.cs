using HospitalManagement.Infrastructure.Identity;
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Controllers;

[Authorize(Roles = HospitalRoles.Admin)]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UsersController(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    public async Task<IActionResult> Index()
    {
        var usersList = await _userManager.Users
            .OrderBy(x => x.Email)
            .ToListAsync();

        var users = new List<(ApplicationUser User, IList<string> Roles)>();

        foreach (var user in usersList)
        {
            var roles = await _userManager.GetRolesAsync(user);
            users.Add((user, roles));
        }

        return View(users);
    }

    [HttpGet]
    public IActionResult Create() => View(new CreateUserViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!HospitalRoles.All.Contains(model.Role)) ModelState.AddModelError(nameof(model.Role), "Choose a valid role.");
        if (!ModelState.IsValid) return View(model);
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, DisplayName = model.DisplayName, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) { foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description); return View(model); }
        await _userManager.AddToRoleAsync(user, model.Role);
        TempData["SuccessMessage"] = "User created successfully.";
        return RedirectToAction(nameof(Index));
    }
}

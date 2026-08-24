using System.ComponentModel.DataAnnotations;
using HospitalManagement.Infrastructure.Identity;

namespace HospitalManagement.Web.Models;

public class CreateUserViewModel
{
    [Required, Display(Name = "Full name")]
    public string DisplayName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = HospitalRoles.Receptionist;
}

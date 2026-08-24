using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
}

using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Mapping;
using HospitalManagement.Application.Services;
using HospitalManagement.Infrastructure;
using HospitalManagement.Infrastructure.Identity;
using HospitalManagement.Application;

var builder = WebApplication.CreateBuilder(args);

// Register Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddApplication();

// Add MVC
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
    await IdentitySeeder.SeedAsync(scope.ServiceProvider, app.Configuration);

app.Run();

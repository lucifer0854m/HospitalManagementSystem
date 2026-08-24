using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Mapping;
using HospitalManagement.Application.Services;
using HospitalManagement.Infrastructure;
using HospitalManagement.Application;
using HospitalManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Register Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddApplication();

// Add MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
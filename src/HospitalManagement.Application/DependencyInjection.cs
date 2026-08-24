using AutoMapper;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Mapping;
using HospitalManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(ApplicationMappingProfile));

        // Services
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IPharmacyService, PharmacyService>();
        services.AddScoped<IBillingService, BillingService>();
        services.AddScoped<IPrescriptionService, PrescriptionService>();
        services.AddScoped<ILaboratoryService, LaboratoryService>();
        services.AddScoped<IReportingService, ReportingService>();

        return services;
    }
}

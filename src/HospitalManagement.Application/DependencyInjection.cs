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

        return services;
    }
}
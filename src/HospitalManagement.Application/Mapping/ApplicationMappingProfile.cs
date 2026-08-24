using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForMember(d => d.FullName,
                o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
            .ForMember(d => d.Gender,
                o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.Status,
                o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<CreatePatientDto, Patient>();

        CreateMap<UpdatePatientDto, Patient>();

        CreateMap<Patient, UpdatePatientDto>();
    }
}